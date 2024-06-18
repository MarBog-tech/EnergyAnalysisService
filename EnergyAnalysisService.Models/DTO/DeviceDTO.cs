using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace EnergyAnalysisService.Models.DTO;

public class DeviceDTO
{
    public Guid Id { get; set;}
    public string Title { get; set; }
    public string Description { get; set; }
    public decimal Wattage { get; set; }
    public Guid CategoryId { get; set; }
    [ValidateNever]
    public string CategoryName { get; set; }
}