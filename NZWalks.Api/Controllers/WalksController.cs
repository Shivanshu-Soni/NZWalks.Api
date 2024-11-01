using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using NZWalks.Api.Models.DomainModels;
using NZWalks.Api.Models.DTO;
using NZWalks.Api.Repository;

namespace NZWalks.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class WalksController : ControllerBase
    {
        private readonly IMapper mapper;
        private readonly IWalkRepository walkRepository;
        public WalksController(IMapper mapper, IWalkRepository walkRepository)
        {
            this.walkRepository = walkRepository;
            this.mapper = mapper;

        }
        //create walk
        //post :/api/walks
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] AddWalksRequestDto addWalksRequestDto)
        {
            //map DTO(AddWalkRequestDto) to domainModel(Walk)
            var walkDomainModel = mapper.Map<Walk>(addWalksRequestDto);
            await walkRepository.CreateAsync(walkDomainModel);


            //map domain model to dto
            return Ok(mapper.Map<WalksDTO>(walkDomainModel));

        }

        //Get: /api/walks
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            // Retrieve all walks from the repository
            var walksDomainModel = await walkRepository.GetAllAsync();

            // Map the domain models to DTOs
            var walksDTO = mapper.Map<List<WalksDTO>>(walksDomainModel);

            // Return the mapped list as an HTTP 200 response
            return Ok(walksDTO);
        }

    }
}