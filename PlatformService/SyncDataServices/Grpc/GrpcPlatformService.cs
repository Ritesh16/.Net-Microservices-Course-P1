using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Grpc.Core;
using PlatformService.Data;

namespace PlatformService.SyncDataServices.Grpc
{
    public class GrpcPlatformService : GrpcPlatform.GrpcPlatformBase
    {
        private readonly IPlatformRepository platformRepository;
        private readonly IMapper mapper;

        public GrpcPlatformService(IPlatformRepository platformRepository, IMapper mapper)
        {
            this.platformRepository = platformRepository;
            this.mapper = mapper;
        }

        public override Task<PlatformResponse> GetAllPlatforms(GetAllRequest request, ServerCallContext context)
        {
            var response = new PlatformResponse();
            var platforms = platformRepository.GetAll();

            foreach(var platform in platforms)
            {
                response.Platform.Add(mapper.Map<GrpcPlatformModel>(platform));
            }

            return Task.FromResult(response);
        }
    }
}