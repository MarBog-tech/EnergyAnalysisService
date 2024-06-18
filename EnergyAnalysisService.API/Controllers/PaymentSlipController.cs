using System.Net;
using System.Security.Claims;
using AutoMapper;
using EnergyAnalysisService.DataAccess.Repository.IRepository;
using EnergyAnalysisService.Models;
using EnergyAnalysisService.Models.DTO;
using EnergyAnalysisService.Models.Entity;
using EnergyAnalysisService.Models.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace EnergyAnalysisService.API.Controllers;

[Route("api/PaymentSlipAPI")]
[ApiController]
public class PaymentSlipController : ControllerBase
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public PaymentSlipController(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    [HttpGet]
    public ActionResult GetPaymentSlips()
    {
        try
        {
            var histories = _unitOfWork.PaymentSlip.GetAll(includeProperties: "Device").ToList();

            if (histories == null || histories.Count == 0)
                return NotFound();

            return Ok(new APIResponse
            {
                StatusCode = HttpStatusCode.OK,
                Result = histories
            });
        }
        catch (Exception ex)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, new APIResponse
            {
                IsSuccess = false,
                ErrorMessages = new List<string> { ex.ToString() }
            });
        }
    }
    
    [HttpGet("{id:guid}", Name = "GetPaymentSlip")]
    public ActionResult GetPaymentSlip(Guid id)
    {
        try
        {
            var paymentSlip = _unitOfWork.PaymentSlip.Get(x => x.Id == id);
            
            if (paymentSlip == null)
                return NotFound();

            return Ok(new APIResponse
            {
                StatusCode = HttpStatusCode.OK,
                Result = paymentSlip
            });
        }
        catch (Exception ex)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, new APIResponse
            {
                IsSuccess = false,
                ErrorMessages = new List<string> { ex.ToString() }
            });
        }
    }
    
    [HttpPost]
    public ActionResult CreatePaymentSlip([FromBody] PaymentSlip paymentSlip)
    {
        try
        {
            var conf = _unitOfWork.Settings.Get(x=> x.UserId == paymentSlip.UserId);
            
            paymentSlip.KilowattPerDay = (_unitOfWork.Device.Get(
                x=>x.Id == paymentSlip.DeviceId)).Wattage * paymentSlip.TimeOfUse;
            paymentSlip.KilowattPerMonth = paymentSlip.KilowattPerDay * 30;
            paymentSlip.KilowattPerYear = paymentSlip.KilowattPerDay * 356;
            paymentSlip.PricePerDay = paymentSlip.KilowattPerDay * conf.PriceForElectricity;
            paymentSlip.PricePerMonth = paymentSlip.KilowattPerMonth * conf.PriceForElectricity;
            paymentSlip.PricePerYear = paymentSlip.KilowattPerYear * conf.PriceForElectricity;
            _unitOfWork.PaymentSlip.Create(paymentSlip);
            _unitOfWork.Save();

            return StatusCode(StatusCodes.Status201Created, new APIResponse
            {
                IsSuccess = true,
                StatusCode = HttpStatusCode.Created
            });
        }
        catch (Exception ex)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, new APIResponse
            {
                IsSuccess = false,
                StatusCode = HttpStatusCode.InternalServerError,
                ErrorMessages = new List<string> { ex.ToString() }
            });
        }
    }

    [HttpDelete("{id:guid}", Name = "DeletePaymentSlip")]
    public ActionResult DeletePaymentSlip(Guid id)
    {
        try
        {
            var paymentSlip = _unitOfWork.PaymentSlip.Get(x => x.Id == id);
            if (paymentSlip == null)
                return NotFound();

            _unitOfWork.PaymentSlip.Delete(paymentSlip);
            _unitOfWork.Save();

            return StatusCode(StatusCodes.Status200OK, new APIResponse
            {
                IsSuccess = true,
                StatusCode = HttpStatusCode.OK
            });
        }
        catch (Exception ex)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, new APIResponse
            {
                IsSuccess = false,
                StatusCode = HttpStatusCode.InternalServerError,
                ErrorMessages = new List<string> { ex.ToString() }
            });
        }
    }
    
    [HttpPut("{id:guid}", Name = "UpdatePaymentSlip")]
    public ActionResult UpdatePaymentSlip(Guid id, [FromBody] PaymentSlip paymentSlip)
    {
        try
        {
            if (paymentSlip == null || id != paymentSlip.Id)
                return BadRequest();

            var conf = _unitOfWork.Settings.Get(x=> x.UserId == paymentSlip.UserId);
            
            paymentSlip.KilowattPerDay = (_unitOfWork.Device.Get(
                x=>x.Id == paymentSlip.DeviceId)).Wattage * paymentSlip.TimeOfUse;
            paymentSlip.KilowattPerMonth = paymentSlip.KilowattPerDay * 30;
            paymentSlip.KilowattPerYear = paymentSlip.KilowattPerDay * 356;
            paymentSlip.PricePerDay = paymentSlip.KilowattPerDay * conf.PriceForElectricity;
            paymentSlip.PricePerMonth = paymentSlip.KilowattPerMonth * conf.PriceForElectricity;
            paymentSlip.PricePerYear = paymentSlip.KilowattPerYear * conf.PriceForElectricity;
            _unitOfWork.PaymentSlip.Update(paymentSlip);
            _unitOfWork.Save();

            return Ok(new APIResponse
            {
                StatusCode = HttpStatusCode.OK,
                IsSuccess = true
            });
        }
        catch (Exception ex)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, new APIResponse
            {
                IsSuccess = false,
                StatusCode = HttpStatusCode.InternalServerError,
                ErrorMessages = new List<string> { ex.ToString() }
            });
        }
    }
}