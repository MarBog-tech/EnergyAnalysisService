using System.Security.Claims;
using EnergyAnalysisService.Client.Services.IServices;
using EnergyAnalysisService.Models;
using EnergyAnalysisService.Models.DTO;
using EnergyAnalysisService.Models.Entity;
using EnergyAnalysisService.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json;

namespace EnergyAnalysisService.Client.Controllers;

public class PaymentSlipController : Controller
{
    private readonly IDeviceService _deviceService;
    private readonly IPaymentSlipService _PaymentSlipService;

    public PaymentSlipController(IDeviceService deviceService, IPaymentSlipService PaymentSlipService)
    {
        _deviceService = deviceService;
        _PaymentSlipService = PaymentSlipService;
    }
    
    public async Task<IActionResult> IndexPaymentSlip()
    {
        List<PaymentSlip> list = new();
        var response = await _PaymentSlipService.GetAllAsync<APIResponse>();
        if (response != null && response.IsSuccess)
        {
            list = JsonConvert.DeserializeObject<List<PaymentSlip>>(Convert.ToString(response.Result));
        }
        return View(list);
    }

    public async Task<IActionResult> CreatePaymentSlip(Guid id)
    {
        var model = new PaymentSlipVM();
        var response = await _deviceService.GetAsync<APIResponse>(id);
        if (response != null && response.IsSuccess)
        {
            model.PaymentSlip = new PaymentSlip()
            {
                DeviceId = id
            };
            model.DeviceName = JsonConvert.DeserializeObject<Device>(Convert.ToString(response.Result)).Title;
            return View(model);
        }
        return BadRequest();
    }
    
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> CreatePaymentSlip(PaymentSlipVM model)
    {
        if (ModelState.IsValid)
        {
            var claimsIdentity = (ClaimsIdentity)User.Identity;
            model.PaymentSlip.UserId = Guid.Parse(claimsIdentity.FindFirst(ClaimTypes.NameIdentifier).Value);
            model.PaymentSlip.TimeOfUse = model.Hours + (decimal)model.Minutes / 60;
            var response = await _PaymentSlipService.CreateAsync<APIResponse>(model.PaymentSlip);
            if (response != null && response.IsSuccess)
            {
                TempData["success"] = "PaymentSlip created successfully";
                return RedirectToAction(nameof(IndexPaymentSlip));
            }
        }
        TempData["error"] = "An error occurred";
        return View(model);
    }

    public async Task<IActionResult> UpdatePaymentSlip(Guid id)
    {
        var response = await _PaymentSlipService.GetAsync<APIResponse>(id);
        if (response != null && response.IsSuccess)
        {
            PaymentSlip paymentSlip = JsonConvert.DeserializeObject<PaymentSlip>(Convert.ToString(response.Result));
            var model = new PaymentSlipVM()
            {
                PaymentSlip = paymentSlip,
                DeviceName = paymentSlip.Device.Title
            };
            return View(model);
        }
        return NotFound();
    }
    
    
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> UpdatePaymentSlip(PaymentSlipVM model)
    {
        if (ModelState.IsValid)
        {
            model.PaymentSlip.TimeOfUse = model.Hours + (decimal)model.Minutes / 60;
            var response = await _PaymentSlipService.UpdateAsync<APIResponse>(model.PaymentSlip);
            if (response != null && response.IsSuccess)
            {
                TempData["success"] = "PaymentSlip updated successfully";
                return RedirectToAction(nameof(IndexPaymentSlip));        
            }
        }
        TempData["error"] = "An error occurred";
        return View(model);
    }
    
    
    public async Task<IActionResult> DeletePaymentSlip(Guid id)
    {
        var response = await _PaymentSlipService.DeleteAsync<APIResponse>(id);
        if (response != null && response.IsSuccess)
        {
            TempData["success"] = "PaymentSlip deleted successfully";
            return RedirectToAction(nameof(IndexPaymentSlip));
        }
        TempData["error"] = "An error occurred";
        return NotFound();
    }
    
}