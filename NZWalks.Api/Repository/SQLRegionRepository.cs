using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using NZWalks.Api.Data;
using NZWalks.Api.Models.DomainModels;

namespace NZWalks.Api.Repository
{
    public class SQLRegionRepository : IRegionRepository
    {
        private readonly NZWalksDbContext nZWalksDbContext;
        public SQLRegionRepository(NZWalksDbContext nZWalksDbContext)
        {
            this.nZWalksDbContext = nZWalksDbContext;
            
        }
        public async Task<List<Region>> GetAllAsync()
        {
            return await nZWalksDbContext.regions.ToListAsync();
            throw new NotImplementedException();
        }
    }
}