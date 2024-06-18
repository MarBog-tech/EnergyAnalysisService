using EnergyAnalysisService.DataAccess.Context;
using EnergyAnalysisService.DataAccess.Repository.IRepository;
using EnergyAnalysisService.Models;
using EnergyAnalysisService.Models.Entity;

namespace EnergyAnalysisService.DataAccess.Repository;

public class SettingsRepository : Repository<Settings>, ISettingsRepository
    {
        private readonly EnergyAnalysisServiceContext _db;
        public SettingsRepository(EnergyAnalysisServiceContext db) : base(db)
        {
            _db = db;
        }
    }