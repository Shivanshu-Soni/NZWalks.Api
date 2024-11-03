using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Xml.Serialization;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;
using NZWalks.Api.Data;
using NZWalks.Api.Models.DomainModels;

namespace NZWalks.Api.Repository
{
    public class SQLWalkRepository : IWalkRepository
    {
        private readonly NZWalksDbContext dbContext;
        public SQLWalkRepository(NZWalksDbContext dbContext)
        {
            this.dbContext = dbContext;

        }
        public async Task<Walk> CreateAsync(Walk walk)
        {
            await dbContext.walks.AddAsync(walk);
            await dbContext.SaveChangesAsync();
            return walk;
        }

        public async Task<Walk?> DeleteAsync(Guid id)
        {
            var walk = await dbContext.walks.FirstOrDefaultAsync(z => z.Id == id);
            if (walk == null)
            {
                return null;
            }
            dbContext.walks.Remove(walk);
            await dbContext.SaveChangesAsync();

            return walk;
        }

        public async Task<List<Walk>> GetAllAsync(string? filterOn = null, string? filterQuery = null,
                                                    string? sortBy = null, bool isAscending = true
                                                    , int pageNumber=1, int pageSize=1000)
        {
            // return await dbContext.walks.Include("difficulty").Include("region").ToListAsync();
            var walks = dbContext.walks.Include("difficulty").Include("region").AsQueryable();
            //Filtering
            if (string.IsNullOrWhiteSpace(filterOn) == false && string.IsNullOrWhiteSpace(filterQuery) == false)
            {
                if (filterOn.Equals("Name", StringComparison.OrdinalIgnoreCase))
                {
                    walks = walks.Where(x => x.Name.Contains(filterQuery));
                }
            }

            //sorting
            if (string.IsNullOrWhiteSpace(sortBy) == false)
            {
                if (sortBy.Equals("Name", StringComparison.OrdinalIgnoreCase))
                {
                    walks = isAscending ? walks.OrderBy(x => x.Name) : walks.OrderByDescending(x => x.Name);
                }
                else if (sortBy.Equals("Length", StringComparison.OrdinalIgnoreCase))
                {
                    walks = isAscending ? walks.OrderBy(x => x.LenghtInkm) : walks.OrderByDescending(x => x.LenghtInkm);
                }
            }

            //Pagination
            var skipResult = (pageNumber-1)*pageSize;// formula

            return await walks.Skip(skipResult).Take(pageSize).ToListAsync();

        }



        public async Task<Walk?> GetByIdAsync(Guid id)
        {
            return await dbContext.walks.Include("difficulty")
            .Include("region")
            .FirstOrDefaultAsync(x => x.Id == id);


        }

        public async Task<Walk?> UpdateAsync(Guid id, Walk walk)
        {
            var existingWalk = await dbContext.walks.FirstOrDefaultAsync(x => x.Id == id);
            if (existingWalk == null)
            {
                return null;
            }
            existingWalk.Name = walk.Name;
            existingWalk.Description = walk.Description;
            existingWalk.LenghtInkm = walk.LenghtInkm;
            existingWalk.WalkImageUrl = walk.WalkImageUrl;
            existingWalk.DifficultyId = walk.DifficultyId;
            existingWalk.regionId = walk.regionId;
            await dbContext.SaveChangesAsync();

            return existingWalk;
        }
    }
}