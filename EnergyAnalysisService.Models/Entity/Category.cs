using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Newtonsoft.Json;

namespace EnergyAnalysisService.Models.Entity;

public class Category
{
    [Key]
    public Guid Id { get; set;}
    public string Name { get; set;}
    [ValidateNever]
    [JsonIgnore]
    public ICollection<Device>? Devices { get; set; }
}