using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using PlatformService.Data;
using PlatformService.Dtos;
using PlatformService.Models;

namespace PlatformService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PlatformsController: ControllerBase
    {
        private readonly IPlatformRepository platformRepository;
        private readonly IMapper mapper;

        public PlatformsController(IPlatformRepository platformRepository, IMapper mapper)
        {
            this.platformRepository = platformRepository;
            this.mapper = mapper;
        }   

        [HttpGet]
        public ActionResult<IEnumerable<PlatformReadDto>> GetPlatforms()
        {
            Console.WriteLine("---> Getting platforms.");
            
            var platforms = this.platformRepository.GetAll();
            
            return Ok(mapper.Map<IEnumerable<PlatformReadDto>>(platforms));
        }

         [HttpGet("{id}", Name = "GetPlatformById")]
        public ActionResult<IEnumerable<PlatformReadDto>> GetPlatformById(int id)
        {
            Console.WriteLine("---> Getting platforms by Id");
            
            var platform = this.platformRepository.GetById(id);
            if(platform == null)
            {
                return NotFound();
            }

            return Ok(mapper.Map<PlatformReadDto>(platform));
        }

        [HttpPost]
        public ActionResult<PlatformReadDto> CreatePlatform(PlatformCreateDto platformCreateDto)
        {
            var platformModel = mapper.Map<Platform>(platformCreateDto);
            platformRepository.Create(platformModel);
            platformRepository.SaveChanges();

            var platformReadDto = mapper.Map<PlatformReadDto>(platformModel);
            return CreatedAtRoute(nameof(GetPlatformById), new { Id =  platformReadDto.Id }, platformReadDto);
        }
    }
}