using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.DataProtection.XmlEncryption;
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

        public async Task<Region> CreateAsync(Region region)
        {
            await nZWalksDbContext.regions.AddAsync(region);
            await nZWalksDbContext.SaveChangesAsync();
            return region;


        }

        public async Task<Region?> DeleteAsync(Guid id)
        {
            var existingregion = await nZWalksDbContext.regions.FirstOrDefaultAsync(x => x.Id == id);
            if (existingregion != null)
            {
                return null;
            }
             nZWalksDbContext.Remove(existingregion);
            await nZWalksDbContext.SaveChangesAsync();
            return existingregion;
        }

        public async Task<List<Region>> GetAllAsync()
        {
            return await nZWalksDbContext.regions.ToListAsync();
            throw new NotImplementedException();
        }

        public Task<Region?> GetByIdAsync(int id)
        {
            return nZWalksDbContext.regions.FirstOrDefaultAsync() ?? throw new NotImplementedException();
            throw new NotImplementedException();
        }

        public async Task<Region?> UpdateAsync(Guid id, Region region)
        {
            var existingRegion = await nZWalksDbContext.regions.FirstOrDefaultAsync(x => x.Id == id);
            if (existingRegion == null)
            {
                return null;
            }
            existingRegion.Code = region.Code;
            existingRegion.Name = region.Name;
            existingRegion.RegionImageUrl = region.RegionImageUrl;
            await nZWalksDbContext.SaveChangesAsync();
            return existingRegion;

        }
    }
}