using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using PlatformService.Data;
using PlatformService.Dtos;
using PlatformService.Models;
using PlatformService.SyncDataServices.Http;

namespace PlatformService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PlatformsController: ControllerBase
    {
        private readonly IPlatformRepository platformRepository;
        private readonly IMapper mapper;
        private readonly ICommandDataClient commandDataClient;

        public PlatformsController(IPlatformRepository platformRepository, 
              IMapper mapper,
              ICommandDataClient commandDataClient)
        {
            this.platformRepository = platformRepository;
            this.mapper = mapper;
            this.commandDataClient = commandDataClient;
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
        public async Task<ActionResult<PlatformReadDto>> CreatePlatform(PlatformCreateDto platformCreateDto)
        {
            var platformModel = mapper.Map<Platform>(platformCreateDto);
            platformRepository.Create(platformModel);
            platformRepository.SaveChanges();

            var platformReadDto = mapper.Map<PlatformReadDto>(platformModel);

            try
            {
                await commandDataClient.SendPlatformToCommand(platformReadDto);
            }
            catch(Exception ex)
            {
                Console.WriteLine($"--> Couldn't send data synchronously : {ex.Message}");
            }


            return CreatedAtRoute(nameof(GetPlatformById), new { Id =  platformReadDto.Id }, platformReadDto);
        }
    }
}