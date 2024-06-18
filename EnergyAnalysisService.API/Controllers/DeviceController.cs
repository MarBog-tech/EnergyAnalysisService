using System.Net;
using AutoMapper;
using EnergyAnalysisService.DataAccess.Repository.IRepository;
using EnergyAnalysisService.Models;
using EnergyAnalysisService.Models.DTO;
using EnergyAnalysisService.Models.Entity;
using Microsoft.AspNetCore.Mvc;

namespace EnergyAnalysisService.API.Controllers;

[Route("api/DeviceAPI")]
[ApiController]
public class DeviceController : ControllerBase
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public DeviceController(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    [HttpGet]
    public ActionResult GetDevices()
    {
        try
        {
            var deviceList = _mapper.Map<List<DeviceDTO>>(_unitOfWork.Device.GetAll(includeProperties: "Category"));

            if (deviceList == null || deviceList.Count == 0)
                return NotFound();

            return Ok(new APIResponse
            {
                StatusCode = HttpStatusCode.OK,
                Result = deviceList
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
    
    [HttpGet("{id:guid}", Name = "GetDevice")]
    public ActionResult GetDevice(Guid id)
    {
        try
        {
            var device = _unitOfWork.Device.Get(x => x.Id == id);

            if (device == null)
                return NotFound();

            return Ok(new APIResponse
            {
                StatusCode = HttpStatusCode.OK,
                Result = device
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
    public ActionResult CreateDevice([FromBody] Device device)
    {
        try
        {
            _unitOfWork.Device.Create(device);
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

    [HttpDelete("{id:guid}", Name = "DeleteDevice")]
    public ActionResult DeleteDevice(Guid id)
    {
        try
        {
            var device = _unitOfWork.Device.Get(x => x.Id == id);
            if (device == null)
                return NotFound();

            _unitOfWork.Device.Delete(device);
            _unitOfWork.Save();

            return NoContent();
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
    
    [HttpPut("{id:guid}", Name = "UpdateDevice")]
    public ActionResult UpdateDevice(Guid id, [FromBody] Device device)
    {
        try
        {
            if (device == null || id != device.Id)
                return BadRequest();

            _unitOfWork.Device.Update(device);
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