using EnergyAnalysisService.Models;

namespace EnergyAnalysisService.Client.Services.IServices;

public interface IBaseService
{
    Task<T> SendAsync<T>(APIRequest apiRequest, bool withBearer = true);
}