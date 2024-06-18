using System.Net;
using EnergyAnalysisService.DataAccess.Repository.IRepository;
using EnergyAnalysisService.Models;
using EnergyAnalysisService.Models.Entity;
using Microsoft.AspNetCore.Mvc;

namespace EnergyAnalysisService.API.Controllers;

[Route("api/CategoryAPI")]
[ApiController]
public class CategoryController : ControllerBase
{
    private readonly IUnitOfWork _unitOfWork;

    public CategoryController(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    [HttpGet]
    public ActionResult GetCategories()
    {
        try
        {
            var categoryList = _unitOfWork.Category.GetAll().ToList();
            if (categoryList == null)
                return NotFound();

            return Ok(new APIResponse
            {
                StatusCode = HttpStatusCode.OK,
                Result = categoryList
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
    
    [HttpGet("{id:guid}", Name = "GetCategory")]
    public ActionResult GetCategory(Guid id)
    {
        try
        {
            var category = _unitOfWork.Category.Get(x => x.Id == id);
            if (category == null)
                return NotFound();

            return Ok(new APIResponse
            {
                StatusCode = HttpStatusCode.OK,
                Result = category
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
    public ActionResult CreateCategory([FromBody] Category category)
    {
        try
        {
            _unitOfWork.Category.Create(category);
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

    [HttpDelete("{id:guid}", Name = "DeleteCategory")]
    public ActionResult DeleteVillaNumber(Guid id)
    {
        try
        {
            var category = _unitOfWork.Category.Get(x => x.Id == id);
            if (category == null)
                return NotFound();

            _unitOfWork.Category.Delete(category);
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
    
    [HttpPut("{id:guid}", Name = "UpdateCategory")]
    public ActionResult UpdateCategory(Guid id, [FromBody] Category category)
    {
        try
        {
            if (category == null || id != category.Id)
                return BadRequest();

            _unitOfWork.Category.Update(category);
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