using EnergyAnalysisService.Models.Entity;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace EnergyAnalysisService.Models.ViewModels;

public class DeviceVM
{
    public Device Device { get; set; }
    [ValidateNever]
    public IEnumerable<SelectListItem> CategoryList { get; set; }
}