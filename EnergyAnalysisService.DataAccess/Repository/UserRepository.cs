using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using AutoMapper;
using EnergyAnalysisService.DataAccess.Context;
using EnergyAnalysisService.DataAccess.Repository.IRepository;
using EnergyAnalysisService.Models.DTO;
using EnergyAnalysisService.Models.Entity;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace EnergyAnalysisService.DataAccess.Repository;

public class UserRepository: IUserRepository
{
    private readonly EnergyAnalysisServiceContext _db;
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly RoleManager<IdentityRole> _roleManager;
    private string secretKey;
    private readonly IMapper _mapper;


    public UserRepository(EnergyAnalysisServiceContext db, IConfiguration configuration, 
        UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager, 
        IMapper mapper)
    {
        _db = db;
        _userManager = userManager;
        _roleManager = roleManager;
        _mapper = mapper;
        this.secretKey = configuration.GetValue<string>("ApiSettings:Secret");
    }

    public bool IsUniqueUser(string username)
    {
        return _db.ApplicationUsers.All(x => x.UserName != username);
    }
    
    public Guid GetUsersId(string username)
    {
        return (_mapper.Map<UserDTO>(
            _db.ApplicationUsers.FirstOrDefaultAsync(u => u.Name == username))).Id;
    }
    
    public async Task<TokenDTO> Login(LoginRequestDTO login)
    {
        var user = _db.ApplicationUsers.FirstOrDefault(x => x.UserName == login.UserName);
        bool isValid = await _userManager.CheckPasswordAsync(user, login.Password);
        if (user is null || isValid)
        {
            return new TokenDTO()
            {
                AccessToken = ""
            };
        }
        var jwtTokenId = $"JTI{Guid.NewGuid()}";
        var accessToken = await GetAccessToken(user,jwtTokenId);
        var refreshToken = await CreateNewRefreshToken(user.Id, jwtTokenId);
        return  new TokenDTO{
            AccessToken = accessToken,
            RefreshToken = refreshToken
        };

    }

    public async Task<UserDTO?> Register(RegistrationRequestDTO registration)
    {
        ApplicationUser user = new()
        {
            UserName = registration.UserName,
            Email=registration.UserName,
            Name = registration.Name
        };
        //При створенні користувача не враховує спец символи у паролі
        var result = await _userManager.CreateAsync(user, registration.Password);
        if (!result.Succeeded) return null;

        if (!await _roleManager.RoleExistsAsync(registration.Role))
        {
            await _roleManager.CreateAsync(new IdentityRole(registration.Role));
        }

        await _userManager.AddToRoleAsync(user, registration.Role);
        var userToReturn = await _db.ApplicationUsers.FirstOrDefaultAsync(u => u.UserName == registration.UserName);
        
        return _mapper.Map<UserDTO>(userToReturn);
    }
    
    private async Task<string> GetAccessToken(ApplicationUser user, string jwtTokenId)
        {
            var roles = await _userManager.GetRolesAsync(user);
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(secretKey);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.NameIdentifier, user.Id),
                    new Claim(ClaimTypes.Name, user.UserName),
                    new Claim(ClaimTypes.Role, roles.FirstOrDefault()),
                    new Claim(JwtRegisteredClaimNames.Jti, jwtTokenId),
                    new Claim(JwtRegisteredClaimNames.Sub, user.Id),
                    new Claim(JwtRegisteredClaimNames.Aud, "dotnetmastery.com")
                }),
                Expires = DateTime.UtcNow.AddMinutes(1),
                Issuer="https://energymaster-api.com",
                Audience="https://test-magic-api.com",
                SigningCredentials = new(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }

    public async Task<TokenDTO> RefreshAccessToken(TokenDTO tokenDTO)
    {
        var existingRefreshToken = await _db.RefreshTokens.FirstOrDefaultAsync(u => u.Refresh_Token == tokenDTO.RefreshToken);
        if (existingRefreshToken == null || !await IsTokenValid(existingRefreshToken, tokenDTO.AccessToken))
        {
            return new TokenDTO();
        }

        if (!existingRefreshToken.IsValid || existingRefreshToken.ExpiresAt < DateTime.UtcNow)
        {
            await MarkTokenAsInvalid(existingRefreshToken);
            return new TokenDTO();
        }

        var newRefreshToken = await CreateNewRefreshToken(existingRefreshToken.UserId, existingRefreshToken.JwtTokenId);
        await MarkTokenAsInvalid(existingRefreshToken);

        var applicationUser = await _db.ApplicationUsers.FindAsync(existingRefreshToken.UserId);
        if (applicationUser == null) return new TokenDTO();

        var newAccessToken = await GetAccessToken(applicationUser, existingRefreshToken.JwtTokenId);

        return new TokenDTO
        {
            AccessToken = newAccessToken,
            RefreshToken = newRefreshToken
        };
    }

    public async Task RevokeRefreshToken(TokenDTO tokenDTO)
    {
        var existingRefreshToken = await _db.RefreshTokens.FirstOrDefaultAsync(_ => _.Refresh_Token == tokenDTO.RefreshToken);
        
        if (existingRefreshToken == null || !await IsTokenValid(existingRefreshToken, tokenDTO.AccessToken))
        {
            return;
        }

        await MarkAllTokensInChainAsInvalid(existingRefreshToken.UserId, existingRefreshToken.JwtTokenId);
    }

    private async Task<string> CreateNewRefreshToken(string userId, string tokenId)
    {
        RefreshToken refreshToken = new()
        {
            IsValid = true,
            UserId = userId,
            JwtTokenId = tokenId,
            ExpiresAt = DateTime.UtcNow.AddMinutes(2),
            Refresh_Token = $"{Guid.NewGuid()}-{Guid.NewGuid()}"
        };

        await _db.RefreshTokens.AddAsync(refreshToken);
        await _db.SaveChangesAsync();
        return refreshToken.Refresh_Token;
    }

    private bool GetAccessTokenData(string accessToken, string expectedUserId, string expectedTokenId)
    {
        try
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var jwt = tokenHandler.ReadJwtToken(accessToken);
            var jwtTokenId = jwt.Claims.FirstOrDefault(u => u.Type == JwtRegisteredClaimNames.Jti)?.Value;
            var userId = jwt.Claims.FirstOrDefault(u => u.Type == JwtRegisteredClaimNames.Sub)?.Value;

            return userId == expectedUserId && jwtTokenId == expectedTokenId;
        }
        catch
        {
            return false;
        }
    }
    
    private async Task MarkAllTokensInChainAsInvalid(string userId, string tokenId)
    {
        await _db.RefreshTokens
            .Where(u => u.UserId == userId && u.JwtTokenId == tokenId)
               .ExecuteUpdateAsync(u => u.SetProperty(refreshToken => refreshToken.IsValid, false));
    }
    
    private Task MarkTokenAsInvalid(RefreshToken refreshToken)
    {
        refreshToken.IsValid = false;
       return _db.SaveChangesAsync();
    }
    
    private async Task<bool> IsTokenValid(RefreshToken refreshToken, string accessToken)
    {
        var isValid = GetAccessTokenData(accessToken, refreshToken.UserId, refreshToken.JwtTokenId);
        if (!isValid)
        {
            await MarkTokenAsInvalid(refreshToken);
        }
        return isValid;
    }
}