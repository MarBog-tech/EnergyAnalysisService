using EnergyAnalysisService.DataAccess.Context;
using EnergyAnalysisService.DataAccess.Repository.IRepository;
using EnergyAnalysisService.Models;
using EnergyAnalysisService.Models.Entity;

namespace EnergyAnalysisService.DataAccess.Repository;

public class CategoryRepository : Repository<Category>, ICategoryRepository
    {
        private readonly EnergyAnalysisServiceContext _db;
        public CategoryRepository(EnergyAnalysisServiceContext db) : base(db)
        {
            _db = db;
        }
    }