using EnergyAnalysisService.DataAccess.Context;
using EnergyAnalysisService.DataAccess.Repository.IRepository;

namespace EnergyAnalysisService.DataAccess.Repository;

public class UnitOfWork : IUnitOfWork
{
    private readonly EnergyAnalysisServiceContext _db;
    public ICategoryRepository Category { get; private set;}
    public IDeviceRepository Device { get; private set;}
    public IPaymentSlipRepository PaymentSlip { get; private set;}
    public ISettingsRepository Settings { get; private set;}

    public UnitOfWork(EnergyAnalysisServiceContext db)
    {
        _db = db;
        PaymentSlip = new PaymentSlipRepository(_db);
        Category = new CategoryRepository(_db);
        Device = new DeviceRepository(_db);
        Settings = new SettingsRepository(_db);
    }


    public void Save()
    {
        _db.SaveChanges();
    }
}