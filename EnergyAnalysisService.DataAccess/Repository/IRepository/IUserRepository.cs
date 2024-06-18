using EnergyAnalysisService.Models.DTO;

namespace EnergyAnalysisService.DataAccess.Repository.IRepository;

public interface IUserRepository
{
    bool IsUniqueUser(string username);
    
   Guid GetUsersId(string username);
    Task<TokenDTO> Login(LoginRequestDTO login);
    Task<UserDTO?> Register(RegistrationRequestDTO registeration);
    Task<TokenDTO> RefreshAccessToken(TokenDTO tokenDTO);

    Task RevokeRefreshToken(TokenDTO tokenDTO);
}