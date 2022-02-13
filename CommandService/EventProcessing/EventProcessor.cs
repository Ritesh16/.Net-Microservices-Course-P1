using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using AutoMapper;
using CommandService.Data;
using CommandService.Dtos;
using CommandService.Models;

namespace CommandService.EventProcessing
{
    public class EventProcessor : IEventProcessor
    {
        private readonly IServiceScopeFactory scopeFactory;
        private readonly IMapper mapper;

        public EventProcessor(IServiceScopeFactory scopeFactory, IMapper mapper)
        {
            this.scopeFactory = scopeFactory;
            this.mapper = mapper;
        }
        public void ProcessEvent(string message)
        {
            var eventType = DetermineEvent(message);

            switch (eventType)
            {
                case EventType.PlatformPublished:
                    // To Do
                    AddPlatform(message);
                    break;
                default:
                    Console.WriteLine("-->");
                    break;
            }
        }

        private void AddPlatform(string platformPublishedMessage)
        {
            using (var scope = scopeFactory.CreateScope())
            {
                var commandRepository = scope.ServiceProvider.GetRequiredService<ICommandRepository>();
                var platformPublishedDto = JsonSerializer.Deserialize<PlatformPublishedDto>(platformPublishedMessage);

                try
                {
                    var platform = mapper.Map<Platform>(platformPublishedDto);
                    if(!commandRepository.ExternalPlatformExists(platform.ExternalId))
                    {
                        commandRepository.CreatePlatform(platform);
                        commandRepository.SaveChanges();
                    }
                    else
                    {
                        Console.WriteLine("--> Platform already exists.");
                    }
                }
                catch(Exception ex)
                {
                    Console.WriteLine($"--> Couldn't add platform to DB : {ex.Message}");
                }
            }
        }
        private EventType DetermineEvent(string notificationMessage)
        {
            Console.WriteLine("--> Determine Event.");

            var eventType = JsonSerializer.Deserialize<GenericEventDto>(notificationMessage);

            switch (eventType.Event)
            {
                case "Platform_Published":
                    Console.WriteLine("-->Platform Published Event Detected.");
                    return EventType.PlatformPublished;
                default:
                    Console.WriteLine("-->Could not determine the event type.");
                    return EventType.Undetermined;
            }
        }
    }

    public enum EventType
    {
        PlatformPublished,
        Undetermined
    }
}