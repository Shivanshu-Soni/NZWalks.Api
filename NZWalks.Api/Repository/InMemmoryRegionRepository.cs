using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using NZWalks.Api.Models.DomainModels;

namespace NZWalks.Api.Repository
{
    public class InMemmoryRegionRepository
    {
        public async Task<List<Region>> GetAllAsync()
        {
            return new List<Region>
            {
                new Region(){
                    Id= Guid.NewGuid(),
                    Code = "BST",
                    Name = "Boston",

                }
            };
        }
    }
}