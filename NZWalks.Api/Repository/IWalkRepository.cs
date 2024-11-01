using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using NZWalks.Api.Models.DomainModels;

namespace NZWalks.Api.Repository
{
    public interface IWalkRepository
    {   
        Task<Walk> CreateAsync(Walk walk);
        
    }
}