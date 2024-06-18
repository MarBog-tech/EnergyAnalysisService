using EnergyAnalysisService.Models;

namespace EnergyAnalysisService.Client.Services.IServices
{
    public interface IApiMessageRequestBuilder
    {
        HttpRequestMessage Build(APIRequest apiRequest);
    }
}
