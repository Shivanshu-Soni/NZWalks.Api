using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
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
            if (ModelState.IsValid)
            {
                //map DTO(AddWalkRequestDto) to domainModel(Walk)
                var walkDomainModel = mapper.Map<Walk>(addWalksRequestDto);
                await walkRepository.CreateAsync(walkDomainModel);


                //map domain model to dto
                return Ok(mapper.Map<WalksDTO>(walkDomainModel));
            }

            return BadRequest(ModelState);



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

        //Get walk by id
        // Get : /api/walk/{id}
        [HttpGet]
        [Route("{id:Guid}")]
        public async Task<IActionResult> GetById([FromRoute] Guid id)
        {
            var walkDomainModel = await walkRepository.GetByIdAsync(id);
            if (walkDomainModel == null)
            {
                return NotFound();
            }

            //map domain to dto
            return Ok(mapper.Map<WalksDTO>(walkDomainModel));
        }

        //update walk by id
        //PUT : /api/walks/{id}
        [HttpPut]
        [Route("{id:Guid}")]
        public async Task<IActionResult> UpdateById([FromRoute] Guid id, UpdateWalkRequiestDto updateWalkRequiestDto)
        {
            if (ModelState.IsValid)
            {
                var walkDomain = mapper.Map<Walk>(updateWalkRequiestDto);
                walkDomain = await walkRepository.UpdateAsync(id, walkDomain);
                if (walkDomain == null)
                {
                    return NotFound();
                }

                return Ok(mapper.Map<WalksDTO>(walkDomain));
            }

            return BadRequest(ModelState);

        }

        // delete walk by id
        // Delete : /api/walks/{id}
        [HttpDelete]
        [Route("{id:Guid}")]
        public async Task<IActionResult> DeleteById([FromRoute] Guid id)
        {
            var deletedwalkDomainModel = await walkRepository.DeleteAsync(id);
            if (deletedwalkDomainModel == null)
            {
                return NotFound();
            }

            return Ok(mapper.Map<WalksDTO>(deletedwalkDomainModel));
        }
    }
}