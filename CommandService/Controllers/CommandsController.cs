using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using CommandService.Data;
using CommandService.Dtos;
using CommandService.Models;
using Microsoft.AspNetCore.Mvc;

namespace CommandService.Controllers
{
    [Route("api/c/platforms/{platformId}/[controller]")]
    [ApiController]
    public class CommandsController : ControllerBase
    {
        private readonly ICommandRepository commandRepository;
        private readonly IMapper mapper;

        public CommandsController(ICommandRepository commandRepository, IMapper mapper)
        {
            this.commandRepository = commandRepository;
            this.mapper = mapper;
        }

        [HttpGet]
        public ActionResult<IEnumerable<CommandReadDto>> GetCommandsForPlatform(int platformId)
        {
            Console.WriteLine($"---> Hit GetCommands for platform: {platformId}");

            if(!commandRepository.PlatformExists(platformId))
            {
                return NotFound();
            }

            var commands = commandRepository.GetCommandsForPlatform(platformId);
            return Ok(mapper.Map<IEnumerable<CommandReadDto>>(commands));
        }

         [HttpGet("{commandId}", Name = "GetCommandForPlatform")]
        public ActionResult<CommandReadDto> GetCommandForPlatform(int platformId, int commandId)
        {
            Console.WriteLine($"---> Hit GetCommand for platform: {platformId} and command: {commandId}");

            if(!commandRepository.PlatformExists(platformId))
            {
                return NotFound();
            }

            var command = commandRepository.GetCommandForPlatform(platformId, commandId);

            if(command == null)
            {
                return NotFound();
            }

            return Ok(mapper.Map<CommandReadDto>(command));
        }
        
        [HttpPost]
        public ActionResult<CommandReadDto> CreateCommandForPlatform(int platformId, CommandCreateDto commandDto)
        {
             Console.WriteLine($"---> Hit CreateCommandForPlatform for platform: {platformId}");

            if(!commandRepository.PlatformExists(platformId))
            {
                return NotFound();
            }

            var command = mapper.Map<Command>(commandDto);
            commandRepository.CreateCommand(platformId, command);
            commandRepository.SaveChanges();

            var commandReadDto = mapper.Map<CommandReadDto>(command);
            return CreatedAtRoute(nameof(GetCommandForPlatform),
                        new {platformId = platformId, commandId = commandReadDto.Id}, commandReadDto);
        }
    }
}