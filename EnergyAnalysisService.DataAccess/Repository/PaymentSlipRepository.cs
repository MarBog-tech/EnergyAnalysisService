using EnergyAnalysisService.DataAccess.Context;
using EnergyAnalysisService.DataAccess.Repository.IRepository;
using EnergyAnalysisService.Models;
using EnergyAnalysisService.Models.Entity;

namespace EnergyAnalysisService.DataAccess.Repository;

public class PaymentSlipRepository : Repository<PaymentSlip>, IPaymentSlipRepository
    {
        private readonly EnergyAnalysisServiceContext _db;
        public PaymentSlipRepository(EnergyAnalysisServiceContext db) : base(db)
        {
            _db = db;
        }
    }