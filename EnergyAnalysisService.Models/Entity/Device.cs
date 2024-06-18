using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace EnergyAnalysisService.Models.Entity;

public class Device
{
    [Key]
    public Guid Id { get; set;}
    public string Title { get; set; }
    public string Description { get; set; }
    [ValidateNever]
    public decimal Wattage { get; set; }
    public Guid CategoryId { get; set; }
    [ForeignKey("CategoryId")]
    [ValidateNever]
    public Category? Category { get; set; }
}