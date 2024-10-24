using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using NZWalks.Api.Models.DomainModels;

namespace NZWalks.Api.Data
{
    public class NZWalksDbContext: DbContext
    {
        public NZWalksDbContext(DbContextOptions dbContextOptions) :base(dbContextOptions)      
        {
            
        }

        public DbSet<Difficulty> difficulties { get; set; }
        public DbSet<Region> regions{ get; set; }
        public DbSet<Walk> walks{ get; set; }
            
        
    }
}