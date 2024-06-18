using EnergyAnalysisService.Client.Services.IServices;
using EnergyAnalysisService.Models;
using EnergyAnalysisService.Models.Entity;
using EnergyAnalysisService.Models.Utility;
using EnergyAnalysisService.Models.ViewModels;

namespace EnergyAnalysisService.Client.Services;

public class PaymentSlipService : IPaymentSlipService
{
    private readonly IHttpClientFactory _httpClient;
    private readonly IBaseService _baseService;
    private string? _url;

    public PaymentSlipService(IHttpClientFactory httpClient, IConfiguration configuration, IBaseService baseService)
    {
        _httpClient = httpClient;
        _baseService = baseService;
        _url = configuration.GetValue<string>("ServiceUrls:API");
    }

    public async Task<T> GetAllAsync<T>()
    {
        return await _baseService.SendAsync<T>(new APIRequest()
        {
            ApiType = SD.ApiType.GET,
            Url = _url + "/api/PaymentSlipAPI"
        });
    }

    public async Task<T> GetAsync<T>(Guid id)
    {
        return await _baseService.SendAsync<T>(new APIRequest()
        {
            ApiType = SD.ApiType.GET,
            Url = _url + "/api/PaymentSlipAPI/" + id
        });
        ;
    }

    public async Task<T> CreateAsync<T>(PaymentSlip? dto)
    {
        return await _baseService.SendAsync<T>(new APIRequest()
        {
            ApiType = SD.ApiType.POST,
            Data = dto,
            Url = _url + "/api/PaymentSlipAPI"
        });
    }

    public async Task<T> UpdateAsync<T>(PaymentSlip dto)
    {
        return await _baseService.SendAsync<T>(new APIRequest()
        {
            ApiType = SD.ApiType.PUT,
            Data = dto,
            Url = _url + "/api/PaymentSlipAPI/" + dto.Id
        });
    }

    public async Task<T> DeleteAsync<T>(Guid id)
    {
        return await _baseService.SendAsync<T>(new APIRequest()
        {
            ApiType = SD.ApiType.DELETE,
            Url = _url + "/api/PaymentSlipAPI/" + id
        });

    }
}