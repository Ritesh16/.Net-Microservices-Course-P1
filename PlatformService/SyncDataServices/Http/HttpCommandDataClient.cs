using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PlatformService.Dtos;
using System.Net.Http;
using System.Text.Json;
using System.Text;

namespace PlatformService.SyncDataServices.Http
{
    public class HttpCommandDataClient : ICommandDataClient
    {
        private readonly HttpClient httpClient;
        private readonly IConfiguration configuration;

        public HttpCommandDataClient(HttpClient httpClient, IConfiguration configuration)
        {
            this.httpClient = httpClient;
            this.configuration = configuration;
        }
        public async Task SendPlatformToCommand(PlatformReadDto platformReadDto)
        {
            var httpContent = new StringContent(
                JsonSerializer.Serialize(platformReadDto),
                Encoding.UTF8,
                "application/json"
            );

            var response = await httpClient.PostAsync($"{configuration["CommandService"]}", httpContent);
            if(response.IsSuccessStatusCode)
            {
                Console.WriteLine("---> Sync Post to CommandService was OK!");
            }
            else
            {
                Console.WriteLine("---> Sync Post to CommandService was not OK!");
            }
        }
    }
}