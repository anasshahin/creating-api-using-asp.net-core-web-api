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
    /*
     The Route attribute is basically defining the root whenever a user enters this root along with the application
     URL, it will be pointed to the region's controller.
     */
    [ApiController]
    /*
     So the ApiController attribute will tell this application that this controller is for API use.
    */
    [Authorize]
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
        public async Task<IActionResult> GetAll() {
            // Task must use if i want to use async
            // Get data from Database - Domain models
            List<Region> regionsDomain = new List<Region>();
            regionsDomain =await regionRepository.GetAllAsync();
            //Map Domain Models to DTOs
            /*  var regionsDto = new List<RegionDto>();
              foreach (var region in regionsDomain)
              {
                  regionsDto.Add(new RegionDto() 
                  {
                    Code = region.Code,
                    Id = region.Id,
                    Name = region.Name,
                    RegionImageUrl = region.RegionImageUrl
                  }

                      );
              }*/
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
        /*
            make sure the ID property [Route ("{id:Guid}")] matches GetById(Guid id). (have the same name)           (have the same name) 
         [Route ("{id:Guid}")] here we put Guid to make type safe
           [FromRoute] we till them that is parameter come from Route
            */
        public async Task<IActionResult> GetById([FromRoute]Guid id)
        {
            // Get data from Database - Domain models
          // var regionsDomain = await dbContext.Regions.FindAsync(id);// find using primary key only 
            //use Find if not async
            // another way
            /*
           var region = dbContext.Regions.FirstOrDefault(x=> x.Id==id);//can find using any property 
            FirstOrDefaultAsync use for async programming
             */
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
        public async Task<IActionResult> Create([FromBody] AddRegionRequestDto addRegionRequestDto ) 
        {
         //   if (ModelState.IsValid)
         //   {
                // Map or Convert DTO to Domain model 
                var regionDomainModel = mapper.Map<Region>(addRegionRequestDto);

                // Use Domain Model to Create Region
                regionDomainModel = await regionRepository.CreateAsync(regionDomainModel);

                // await dbContext.Regions.AddAsync(regionDomainModel);
                // use Add if not async
                // await dbContext.SaveChangesAsync();
                // use SaveChanges if not async

                // Map Domain model back to DTO
                var regionDto = mapper.Map<RegionDto>(regionDomainModel);
                return CreatedAtAction(nameof(GetById), new { id = regionDto.Id }, regionDto);

         //   }
           // else { 
           // return BadRequest(ModelState);
           // }
            
        }
        // Update
        // Put : https://localhost:portnumber/api/regions{id}
        [HttpPut]
        [Route("{id}")]
        [ValidateModel]
        public async Task<IActionResult> Update([FromRoute] Guid id, [FromBody] UpdateRegionRequestDto updateRegion )
        {
           // if (ModelState.IsValid)
         //   {
                // Map DTO to Domain Model
                var regionsDomainModel = mapper.Map<Region>(updateRegion);
                // Check if region exists
                regionsDomainModel = await regionRepository.UpdateAsync(regionsDomainModel, id);
                if (regionsDomainModel == null)
                {
                    return NotFound();
                }
                // Update region
                // Map DTO to domain model
                //  regionsDomainModel.Name = updateRegion.Name;
                //regionsDomainModel.RegionImageUrl = updateRegion.RegionImageUrl;
                //regionsDomainModel.Code = updateRegion.Code;
                //await dbContext.SaveChangesAsync();

                // Convert Domain Model to DTO
                var regionDto = mapper.Map<RegionDto>(regionsDomainModel);
                // return updated region

                return Ok(regionDto);
          //  }
          //  else {
           // return BadRequest(ModelState);
        //    }
            
        }
        // Delete
        // Delete : https://localhost:portnumber/api/regions/{id}
        [HttpDelete]
        [Route("{id}")]
        public async Task<IActionResult> Delete([FromRoute] Guid id) {
            var regionDomainModel =await regionRepository.DeleteAsync(id);

            if (regionDomainModel == null)
            {
                return NotFound();
            }

            // Delete region
            // dbContext.Regions.Remove(regionDomainModel);
            // Remove Don't have async version like others
            //  await dbContext.SaveChangesAsync();
            // Convert Domain Model to DTO
            
            // return deleted region
            return Ok(mapper.Map<RegionDto>(regionDomainModel));
        }
    }
}
