using EnergyAnalysisService.Models;
using EnergyAnalysisService.Models.Entity;

namespace EnergyAnalysisService.Client.Services.IServices;

public interface IDeviceService
{
    Task<T> GetAllAsync<T>();
    Task<T> GetAsync<T>(Guid id);
    Task<T> CreateAsync<T>(Device dto);
    Task<T> UpdateAsync<T>(Device dto);
    Task<T> DeleteAsync<T>(Guid id);
}