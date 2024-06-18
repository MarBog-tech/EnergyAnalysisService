using System.ComponentModel.DataAnnotations;

namespace EnergyAnalysisService.Models.Entity;

public class Settings
{
    [Key]
    public Guid Id { get; set; }
    public Guid UserId { get; set; }
    public decimal PriceForElectricity { get; set; }
}