using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using NZWalks.Api.Models.DomainModels;

namespace NZWalks.Api.Repository
{
    public interface IRegionRepository
    {
        Task<List<Region>> GetAllAsync();
        Task<Region?> GetByIdAsync(int id);
        Task<Region?> UpdateAsync(Guid id, Region region) ;
        Task<Region> CreateAsync(Region region); 

        Task<Region?> DeleteAsync(Guid id) ;

        
    }
}