using EnergyAnalysisService.Models;
using EnergyAnalysisService.Models.Entity;

namespace EnergyAnalysisService.Client.Services.IServices;

public interface ICategoryService
{
    Task<T> GetAllAsync<T>();
    Task<T> GetAsync<T>(Guid id);
    Task<T> CreateAsync<T>(Category dto);
    Task<T> UpdateAsync<T>(Category dto);
    Task<T> DeleteAsync<T>(Guid id);
}