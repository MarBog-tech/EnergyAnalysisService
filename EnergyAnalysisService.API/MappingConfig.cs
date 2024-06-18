using AutoMapper;
using EnergyAnalysisService.Models.DTO;
using EnergyAnalysisService.Models.Entity;

namespace EnergyAnalysisService.API;

public class MappingConfig : Profile
{
    public MappingConfig()
    {
        CreateMap<Device, DeviceDTO>()
            .ForMember(dest => dest.CategoryName, 
                opt => opt.MapFrom(
                    src => src.Category.Name));
        
        CreateMap<ApplicationUser, UserDTO>().ReverseMap();
    }
}