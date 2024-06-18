namespace EnergyAnalysisService.DataAccess.Repository.IRepository;

public interface IUnitOfWork
{
    ICategoryRepository Category { get; }
    IDeviceRepository Device { get;}
    IPaymentSlipRepository PaymentSlip { get;}
    ISettingsRepository Settings { get;}
    void Save();
}