using System.Net;
using EnergyAnalysisService.DataAccess.Repository.IRepository;
using EnergyAnalysisService.Models;
using EnergyAnalysisService.Models.DTO;
using EnergyAnalysisService.Models.Entity;
using Microsoft.AspNetCore.Mvc;

namespace EnergyAnalysisService.API.Controllers;

[Route("api/UsersAuth")]
[ApiController]
public class UsersController : ControllerBase
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IUserRepository _userRepo;
    public UsersController(IUserRepository userRepo, IUnitOfWork unitOfWork)
    {
        _userRepo = userRepo;
        _unitOfWork = unitOfWork;
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginRequestDTO model)
    {
        var tokenDto = await _userRepo.Login(model);
        if (string.IsNullOrEmpty(tokenDto?.AccessToken))
            return BadRequest(new APIResponse
            {
                StatusCode = HttpStatusCode.BadRequest,
                IsSuccess = false,
                ErrorMessages = { "Username or password is incorrect" }
            });

        return Ok(new APIResponse
        {
            StatusCode = HttpStatusCode.OK,
            IsSuccess = true,
            Result = tokenDto
        });
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] RegistrationRequestDTO model)
    {
        bool isUserNameUnique = _userRepo.IsUniqueUser(model.UserName);
        if (!isUserNameUnique)
            return BadRequest(new APIResponse
            {
                StatusCode = HttpStatusCode.BadRequest,
                IsSuccess = false,
                ErrorMessages = { "Username already exists" }
            });

        var user = await _userRepo.Register(model);
        if (user == null)
            return BadRequest(new APIResponse
            {
                StatusCode = HttpStatusCode.BadRequest,
                IsSuccess = false,
                ErrorMessages = { "Error while registering" }
            });
        _unitOfWork.Settings.Create(new Settings()
        {
            UserId = user.Id,
            PriceForElectricity = 2.64m
        });
        _unitOfWork.Save();
        return Ok(new APIResponse
        {
            StatusCode = HttpStatusCode.OK,
            IsSuccess = true
        });
    }

    [HttpPost("refresh")]
    public async Task<IActionResult> GetNewTokenFromRefreshToken([FromBody] TokenDTO tokenDTO)
    {
        if (!ModelState.IsValid)
            return BadRequest(new APIResponse { IsSuccess = false, Result = "Invalid Input" });

        var tokenDTOResponse = await _userRepo.RefreshAccessToken(tokenDTO);
        if (string.IsNullOrEmpty(tokenDTOResponse?.AccessToken))
            return BadRequest(new APIResponse
            {
                StatusCode = HttpStatusCode.BadRequest,
                IsSuccess = false,
                ErrorMessages = { "Token Invalid" }
            });

        return Ok(new APIResponse
        {
            StatusCode = HttpStatusCode.OK,
            IsSuccess = true,
            Result = tokenDTOResponse
        });
    }
    
    [HttpPost("revoke")]
    public async Task<IActionResult> RevokeRefreshToken([FromBody] TokenDTO tokenDTO)
    {
        if (!ModelState.IsValid)
            return BadRequest(new APIResponse { IsSuccess = false, Result = "Invalid Input" });

        await _userRepo.RevokeRefreshToken(tokenDTO);
        return Ok(new APIResponse { IsSuccess = true, StatusCode = HttpStatusCode.OK });
    }
}

