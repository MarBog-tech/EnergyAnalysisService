using EnergyAnalysisService.Client.Services.IServices;
using EnergyAnalysisService.Models;
using EnergyAnalysisService.Models.Entity;
using EnergyAnalysisService.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace EnergyAnalysisService.Client.Controllers;

public class ElectricalLoadController : Controller
{
    private readonly IDeviceService _deviceService;

    public ElectricalLoadController(IDeviceService deviceService)
    {
        _deviceService = deviceService;
    }

    // GET
    public IActionResult Index()
    {
        return View();
    }
    
    [HttpPost]
    public async Task<IActionResult> SearchDevices(string searchTerm, decimal energySourceCapacity)
    {
        List<Device> list = new();
        var response = await _deviceService.GetAllAsync<APIResponse>();
        if (response != null && response.IsSuccess)
        {
            list = JsonConvert.DeserializeObject<List<Device>>(Convert.ToString(response.Result));
        }
        
        return PartialView("_DeviceSearchResults",  list);
    }
}
        // var devices = _context.Devices
        //     .Where(d => d.Name.Contains(searchTerm))
        //     .ToList();
        //
        // var filteredDevices = devices.Where(d => d.Kilowatt <= energySourceCapacity).ToList();
