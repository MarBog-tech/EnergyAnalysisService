using EnergyAnalysisService.Models.DTO;

namespace EnergyAnalysisService.Client.Services.IServices
{
    public interface ITokenProvider
    {
        void SetToken(TokenDTO tokenDTO);
        TokenDTO? GetToken();
        void ClearToken();
    }
}
