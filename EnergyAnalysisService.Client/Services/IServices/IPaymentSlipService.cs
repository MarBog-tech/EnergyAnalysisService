using EnergyAnalysisService.Models;
using EnergyAnalysisService.Models.Entity;
using EnergyAnalysisService.Models.ViewModels;

namespace EnergyAnalysisService.Client.Services.IServices;

public interface IPaymentSlipService
{
    Task<T> GetAllAsync<T>();
    Task<T> GetAsync<T>(Guid id);
    Task<T> CreateAsync<T>(PaymentSlip? dto);
    Task<T> UpdateAsync<T>(PaymentSlip dto);
    Task<T> DeleteAsync<T>(Guid id);
}