using EnergyAnalysisService.Client.Services.IServices;
using EnergyAnalysisService.Models;
using EnergyAnalysisService.Models.DTO;
using EnergyAnalysisService.Models.Entity;
using EnergyAnalysisService.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json;

namespace EnergyAnalysisService.Client.Controllers;

public class DeviceController : Controller
{
    private readonly IDeviceService _deviceService;
    private readonly ICategoryService _categoryService;

    public DeviceController(IDeviceService deviceService, ICategoryService categoryService)
    {
        _deviceService = deviceService;
        _categoryService = categoryService;
    }
    
    public async Task<IActionResult> IndexDevice()
    {
        List<DeviceVM> list = new();
        var response = await _deviceService.GetAllAsync<APIResponse>();
        if (response != null && response.IsSuccess)
        {
            list = JsonConvert.DeserializeObject<List<DeviceVM>>(Convert.ToString(response.Result));
        }
        return View(list);
    }

    public async Task<IActionResult> CreateDevice()
    {
        DeviceVM deviceVm = new DeviceVM();
        var response = await _categoryService.GetAllAsync<APIResponse>();
        
        if (response != null && response.IsSuccess)
        {
            deviceVm.CategoryList = JsonConvert.DeserializeObject<List<Category>>(Convert.ToString(response.Result))
                .Select(i => new SelectListItem
                {
                    Text = i.Name,
                    Value = i.Id.ToString()
                });
        }
        return View(deviceVm);
    }
    
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> CreateDevice(DeviceVM model)
    {
        if (ModelState.IsValid)
        {
            var response = await _deviceService.CreateAsync<APIResponse>(model.Device);
            if (response != null && response.IsSuccess)
            {
                TempData["success"] = "Device created successfully";
                return RedirectToAction(nameof(IndexDevice));
            }
        }
        
        TempData["error"] = "An error occurred";
        var resp = await _categoryService.GetAllAsync<APIResponse>();
        if (resp != null && resp.IsSuccess)
        {
            model.CategoryList = JsonConvert.DeserializeObject<List<Category>>(Convert.ToString(resp.Result))
                .Select(i => new SelectListItem
                {
                    Text = i.Name,
                    Value = i.Id.ToString()
                });
        }
        return View(model);
        
    }

    public async Task<IActionResult> UpdateDevice(Guid deviceId)
    {
        DeviceVM deviceVm = new DeviceVM();
        
        var response = await _deviceService.GetAsync<APIResponse>(deviceId);
        if (response != null && response.IsSuccess)
        {
            deviceVm.Device = JsonConvert.DeserializeObject<Device>(Convert.ToString(response.Result));
        }
        
        response = await _categoryService.GetAllAsync<APIResponse>();
        if (response != null && response.IsSuccess)
        {
            deviceVm.CategoryList = JsonConvert.DeserializeObject<List<Category>>(Convert.ToString(response.Result))
                .Select(i => new SelectListItem
                {
                    Text = i.Name,
                    Value = i.Id.ToString()
                });
        }
        
        return View(deviceVm);
    }
    
    
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> UpdateDevice(DeviceVM model)
    {
        if (ModelState.IsValid)
        {
            var response = await _deviceService.UpdateAsync<APIResponse>(model.Device);
            if (response != null && response.IsSuccess)
            {
                TempData["success"] = "Device updated successfully";
                return RedirectToAction(nameof(IndexDevice));        
            }
        }
        
        TempData["error"] = "An error occurred";
        var resp = await _categoryService.GetAllAsync<APIResponse>();
        if (resp != null && resp.IsSuccess)
        {
            model.CategoryList = JsonConvert.DeserializeObject<List<Category>>(Convert.ToString(resp.Result))
                .Select(i => new SelectListItem
                {
                    Text = i.Name,
                    Value = i.Id.ToString()
                });
        }
        return View(model);
    }
    
    [HttpDelete]
    public async Task<IActionResult> DeleteDevice(Guid id)
    {
        var response = await _deviceService.DeleteAsync<APIResponse>(id);
        if (response != null && response.IsSuccess)
        {
            return Json(new { success = true, message = "Delete Successful" });;
        }
        return Json(new { success = false, message = "Error while deleting" });
    }
    
    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        List<DeviceDTO> list = new();
        var response = await _deviceService.GetAllAsync<APIResponse>();
        if (response != null && response.IsSuccess)
        {
            list = JsonConvert.DeserializeObject<List<DeviceDTO>>(Convert.ToString(response.Result));
        }
        return Json(new { data = list });
    }
    
}