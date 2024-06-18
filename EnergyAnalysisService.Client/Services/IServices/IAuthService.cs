using EnergyAnalysisService.Models.DTO;

namespace EnergyAnalysisService.Client.Services.IServices
{
    public interface IAuthService
    {
        Task<T> LoginAsync<T>(LoginRequestDTO objToCreate);
        Task<T> RegisterAsync<T>(RegistrationRequestDTO objToCreate);
        Task<T> LogoutAsync<T>(TokenDTO obj);
    }
}
