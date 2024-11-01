using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using NZWalks.Api.CustomActionFilters;
using NZWalks.Api.Data;
using NZWalks.Api.Models.DomainModels;
using NZWalks.Api.Models.DTO;
using NZWalks.Api.Repository;

namespace NZWalks.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class RegionsController : ControllerBase
    {
        public NZWalksDbContext _dbContext;
        private readonly IRegionRepository regionRepository;

        public readonly IMapper mapper;

        public RegionsController(NZWalksDbContext dbContext, IRegionRepository regionRepository, IMapper mapper)
        {
            this.mapper = mapper;
            this.regionRepository = regionRepository;
            this._dbContext = dbContext;
        }
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            // var region = new List<Region>
            // {
            //     new Region {Id=Guid.NewGuid(),
            //     Name= "Auckland region",
            //     Code="AKL",
            //     RegionImageUrl="https://www.istockphoto.com/photo/2018-jan-3-auckland-new-zealand-panorama-view-beautiful-landcape-of-the-building-in-gm1060826424-283569015"}
            // };

            var regions = await regionRepository.GetAllAsync();

            //Map domain model to DTO
            // var regionDto = new List<RegionDto>();
            // foreach (var region in regions)
            // {
            //     regionDto.Add(new RegionDto()
            //     {
            //         Id = region.Id,
            //         Code = region.Code,
            //         Name = region.Name,
            //         RegionImageUrl = region.RegionImageUrl,
            //     });
            // }
            // using autommaper to bind domainmodel and dto
            var regionsdto = mapper.Map<List<RegionDto>>(regions);
            // return dto
            return Ok(regionsdto);
        }

        // get single region
        //GET:https://localhost:portnumber/api/regions/{id}
        [HttpGet]
        [Route("id")]
        public async Task<IActionResult> GetRegionById([FromRoute] int id)
        {
            // find applied only to primary key
            //var region =  await _dbContext.regions.Find(id);
            var region = await regionRepository.GetByIdAsync(id);
            if (region == null)
            {
                return NotFound();
            }

            // map regiondomain model to region to dto
            // var regionDto = new RegionDto
            // {
            //     Id = region.Id,
            //     Code = region.Code,
            //     Name = region.Name,
            //     RegionImageUrl = region.RegionImageUrl,
            // };

            var regionDto = mapper.Map<RegionDto>(region);
            return Ok(regionDto);

        }

        //POST: To create new record
        [HttpPost]
        [ValidateModel]
        public async Task<IActionResult> CreateRegion([FromBody] AddRegionRequestDto addRegionDto)
        {
          
                var regionDomainModel = mapper.Map<Region>(addRegionDto);

            // use dbcontext to create record
            await regionRepository.CreateAsync(regionDomainModel);
            await _dbContext.SaveChangesAsync();

            // var regionDto = new RegionDto
            // {
            //     Id = regionDomainModel.Id,
            //     Code = regionDomainModel.Code,
            //     Name = regionDomainModel.Name,
            //     RegionImageUrl = regionDomainModel.RegionImageUrl,
            // };
            // using automapper to map region to regiondto 
            var regionDto = mapper.Map<RegionDto>(regionDomainModel);


            return CreatedAtAction(nameof(GetRegionById), new { id = regionDomainModel.Id }, regionDto);
            

          
            

        }

        [HttpPut]
        [Route("{id:Guid}")]
        [ValidateModel]
        public async Task<IActionResult> updateRegion([FromRoute] Guid Id, [FromBody] UpdateRegionRequestDto updateRegionRequestDto)
        {
            

            var regionDomainModel = new Region
            {
                Code = updateRegionRequestDto.Code,
                Name = updateRegionRequestDto.Name,
                RegionImageUrl = updateRegionRequestDto.RegionImageUrl,

            };

            // var regionDomainModel =  await _dbContext.regions.FirstOrDefaultAsync(x => x.Id == Id);
            regionDomainModel = await regionRepository.UpdateAsync(Id, regionDomainModel);

            if (regionDomainModel == null)
            {
                return NotFound("Region does not exist");

            }

            // convert domainmodel to dto
            var regionDto = new RegionDto
            {
                Id = regionDomainModel.Id,
                Code = regionDomainModel.Code,
                Name = regionDomainModel.Name,
                RegionImageUrl = regionDomainModel.RegionImageUrl,

            };

            return Ok(regionDto);


        }

        [HttpDelete]
        [Route("{id:Guid}")]
        public async Task<IActionResult> Deleteregion([FromRoute] Guid id)
        {
            var region = await regionRepository.DeleteAsync(id);

            if (region == null)
            {
                return NotFound("No region to delete");

            }
            // return deleted region back 
            ///map domain model to dto

            var regiondto = new RegionDto
            {
                Id = region.Id,
                Code = region.Code,
                Name = region.Name,
                RegionImageUrl = region.RegionImageUrl,

            };
            return Ok(regiondto);

        }
    }
}