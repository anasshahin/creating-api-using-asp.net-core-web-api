using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NZWalksAPI.CustomActionFilters;
using NZWalksAPI.Data;
using NZWalksAPI.Models.DIO;
using NZWalksAPI.Models.Domain;
using NZWalksAPI.Models.DTO;
using NZWalksAPI.Repositories;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace NZWalksAPI.Controllers
{
    [Route("api/[controller]")]
    
    [ApiController]
   
    public class RegionsController : ControllerBase
    {
        private readonly NZWalkDbContext dbContext;
        private readonly IRegionRepository regionRepository;
        private readonly IMapper mapper;

        public RegionsController(NZWalkDbContext dbContext,IRegionRepository regionRepository
            ,IMapper mapper)
        {
            this.dbContext = dbContext;
            this.regionRepository = regionRepository;
            this.mapper = mapper;
        }
        // GET ALL REGIONS
        // GET: https://localhost:portnumber/api/regions
        [HttpGet]
        [Authorize (Roles = "Reader")]
        public async Task<IActionResult> GetAll() {
           
            List<Region> regionsDomain = new List<Region>();
            regionsDomain =await regionRepository.GetAllAsync();
           
            
            //Map Domain Models to DTOs
            var regionsDto = mapper.Map<List<RegionDto>> (regionsDomain);
            // Return DTOs
            return Ok(regionsDto);
        }
        //  IActionResult is the response type.

        // GET SINGLE REGION (Get Region by id)
        // GET: https://localhost:portnumber/api/regions/{id}

        [HttpGet]
        [Route ("{id:Guid}")]

        [Authorize(Roles = "Reader")]
        public async Task<IActionResult> GetById([FromRoute]Guid id)
        {
          
            var regionsDomain= await regionRepository.GetByIdAsync(id);
            if (regionsDomain == null)
            {
                return NotFound();
            }  
            // Return DTO
            return Ok(mapper.Map<RegionDto>(regionsDomain));
        }
        // POST To create new region
        // POST : https://localhost:portnumber/api/regions
        [HttpPost]
        [ValidateModel]
        [Authorize(Roles = "Writer")]

        public async Task<IActionResult> Create([FromBody] AddRegionRequestDto addRegionRequestDto ) 
        {
         //   if (ModelState.IsValid)
         //   {
                // Map or Convert DTO to Domain model 
                var regionDomainModel = mapper.Map<Region>(addRegionRequestDto);

                // Use Domain Model to Create Region
                regionDomainModel = await regionRepository.CreateAsync(regionDomainModel);

              
                // Map Domain model back to DTO
                var regionDto = mapper.Map<RegionDto>(regionDomainModel);
                return CreatedAtAction(nameof(GetById), new { id = regionDto.Id }, regionDto);

         
            
        }
        // Update
        // Put : https://localhost:portnumber/api/regions{id}
        [HttpPut]
        [Route("{id}")]
        [ValidateModel]
        [Authorize(Roles = "Writer")]
        public async Task<IActionResult> Update([FromRoute] Guid id, [FromBody] UpdateRegionRequestDto updateRegion )
        {
          
                // Map DTO to Domain Model
                var regionsDomainModel = mapper.Map<Region>(updateRegion);
                // Check if region exists
                regionsDomainModel = await regionRepository.UpdateAsync(regionsDomainModel, id);
                if (regionsDomainModel == null)
                {
                    return NotFound();
                }
                // Update region

                // Convert Domain Model to DTO
                var regionDto = mapper.Map<RegionDto>(regionsDomainModel);
                // return updated region

                return Ok(regionDto);
            
        }
        // Delete
        // Delete : https://localhost:portnumber/api/regions/{id}
        [HttpDelete]
        [Route("{id}")]
        [Authorize(Roles = "Writer")]
        public async Task<IActionResult> Delete([FromRoute] Guid id) {
            var regionDomainModel =await regionRepository.DeleteAsync(id);

            if (regionDomainModel == null)
            {
                return NotFound();
            }

            // Delete region
            
            // Convert Domain Model to DTO
            
            // return deleted region
            return Ok(mapper.Map<RegionDto>(regionDomainModel));
        }
    }
}
