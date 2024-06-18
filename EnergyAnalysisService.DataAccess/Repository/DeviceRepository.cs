using EnergyAnalysisService.DataAccess.Context;
using EnergyAnalysisService.DataAccess.Repository.IRepository;
using EnergyAnalysisService.Models;
using EnergyAnalysisService.Models.Entity;

namespace EnergyAnalysisService.DataAccess.Repository;

public class DeviceRepository : Repository<Device>, IDeviceRepository
    {
        private readonly EnergyAnalysisServiceContext _db;
        public DeviceRepository(EnergyAnalysisServiceContext db) : base(db)
        {
            _db = db;
        }
    }