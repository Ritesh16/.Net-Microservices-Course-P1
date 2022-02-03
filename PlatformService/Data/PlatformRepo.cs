using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PlatformService.Models;

namespace PlatformService.Data
{
    public class PlatformRepository : IPlatformRepository
    {
        private readonly AppDbContext context;

        public PlatformRepository(AppDbContext context)
        {
            this.context = context;
        }
        public void Create(Platform platform)
        {
            if(platform == null)
            {
                throw new ArgumentException(nameof(platform));
            }

            context.Platforms.Add(platform);
        }

        public IEnumerable<Platform> GetAll()
        {
            return context.Platforms.ToList();
        }

        public Platform GetById(int id)
        {
            return context.Platforms.FirstOrDefault(p => p.Id == id);
        }

        public bool SaveChanges()
        {
            return context.SaveChanges() >= 0;
        }
    }
}