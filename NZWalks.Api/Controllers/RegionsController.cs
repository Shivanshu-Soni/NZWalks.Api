using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using NZWalks.Api.Data;
using NZWalks.Api.Models.DomainModels;
using NZWalks.Api.Models.DTO;

namespace NZWalks.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class RegionsController : ControllerBase
    {
        public NZWalksDbContext _dbContext;
        public RegionsController(NZWalksDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        [HttpGet]
        public IActionResult GetAll()
        {
            // var region = new List<Region>
            // {
            //     new Region {Id=Guid.NewGuid(),
            //     Name= "Auckland region",
            //     Code="AKL",
            //     RegionImageUrl="https://www.istockphoto.com/photo/2018-jan-3-auckland-new-zealand-panorama-view-beautiful-landcape-of-the-building-in-gm1060826424-283569015"}
            // };

            var regions = _dbContext.regions.ToList();
             var regionDto = new List<RegionDto>();
            //Map domain model to DTO

            // return dto
            return Ok(regions);
        }

        [HttpGet]
        [Route("id")]
        public IActionResult GetRegionById([FromRoute] int id)
        {
            var region = _dbContext.regions.Find(id);
            if (region == null)
            {
                return NotFound();
            }
            return Ok(region);

        }
    }
}