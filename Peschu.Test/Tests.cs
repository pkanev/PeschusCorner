namespace Peschu.Test
{
    using AutoMapper;
    using Microsoft.EntityFrameworkCore;
    using Peschu.Data;
    using Peschu.Web.Infrastructure.Mapping;
    using System;

    public class Tests
    {
        private static bool testsInitialized = false;

        public static void Initialilze()
        {
            if(!testsInitialized)
            {
                Mapper.Initialize(config => config.AddProfile<AutoMapperProfile>());
                testsInitialized = true;
            }
        }

        public static PeschuDbContext GetDatabase()
        {
            var dbOptions = new DbContextOptionsBuilder<PeschuDbContext>().UseInMemoryDatabase(Guid.NewGuid().ToString()).Options;
            return new PeschuDbContext(dbOptions);
        }
    }
}
