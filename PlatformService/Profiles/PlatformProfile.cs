using AutoMapper;
using PlatformService.Dtos;
using PlatformService.Models;

namespace PlatformService.Profiles
{
    public class PlatformProfile : Profile
    {
        public PlatformProfile()
        {
            // Source --> Target
            CreateMap<PlatformCreateDto, Platform>();
            CreateMap<Platform, PlatformReadDto>();
            CreateMap<PlatformReadDto, PlatformPublishedDto>();
            CreateMap<Platform, GrpcPlatformModel>()
                    .ForMember(dest => dest.PlatformId, 
                               opt => opt.MapFrom(src => src.Id))
                    .ForMember(dest => dest.Publisher, 
                               opt => opt.MapFrom(src => src.Publisher))
                    .ForMember(dest => dest.Name, 
                               opt => opt.MapFrom(src => src.Name));
        }
    }
}