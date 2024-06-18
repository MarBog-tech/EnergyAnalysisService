using EnergyAnalysisService.Models.Entity;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace EnergyAnalysisService.Models.ViewModels;

public class PaymentSlipVM
{
    [ValidateNever]
    public PaymentSlip? PaymentSlip { get; set; }
    
    [ValidateNever]
    public string DeviceName { get; set; }
    public int Hours { get; set; }
    public int Minutes { get; set; }
}