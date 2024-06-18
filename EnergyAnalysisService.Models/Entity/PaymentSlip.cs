using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace EnergyAnalysisService.Models.Entity;

public class PaymentSlip
{
    [Key]
    public Guid Id { get; set;}
    public Guid UserId { get; set; }
    public Guid DeviceId { get; set; }
    [ForeignKey("DeviceId")]
    [ValidateNever]
    public Device? Device { get; set;}

    public decimal TimeOfUse { get; set; }

    public decimal KilowattPerDay { get; set;}
    public decimal KilowattPerMonth { get; set; }
    public decimal KilowattPerYear { get; set; }
    
    public decimal PricePerDay { get; set; }
    public decimal PricePerMonth { get; set; }
    public decimal PricePerYear { get; set; }
    
}