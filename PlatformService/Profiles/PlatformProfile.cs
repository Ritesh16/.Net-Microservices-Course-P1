using AutoMapper;
using PlatformService.Dtos;
using PlatformService.Models;

namespace PlatformService.Profiles
{
    public class PlatformProfile: Profile
    {
        public PlatformProfile()
        {
            // Source --> Target
            CreateMap<PlatformCreateDto, Platform>();
            CreateMap<Platform, PlatformReadDto>();
            CreateMap<PlatformReadDto, PlatformPublishedDto>();
        }
    }
}