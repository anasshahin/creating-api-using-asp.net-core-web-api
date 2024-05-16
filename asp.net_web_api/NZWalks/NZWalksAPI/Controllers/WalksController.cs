using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NZWalksAPI.CustomActionFilters;
using NZWalksAPI.Models.Domain;
using NZWalksAPI.Models.DTO;
using NZWalksAPI.Repositories;

namespace NZWalksAPI.Controllers
{
    // /api/walks
    [Route("api/[controller]")]
    [ApiController]
    public class WalksController : ControllerBase
    {
        private readonly IMapper mapper;
        private readonly IWalkRepository walkRepository;

        public WalksController(IMapper mapper,IWalkRepository walkRepository)
        {
            this.mapper = mapper;
            this.walkRepository = walkRepository;
        }
        // CREATE Walks
        // POST: /api/walks
        [HttpPost]
        [ValidateModel]
        public async Task<IActionResult> Create([FromBody] AddWalkRequestDto addWalkRequestDto) 
        {
           
                // Map DTO to Domain Model
                var walkDomainModel = mapper.Map<Walk>(addWalkRequestDto);

                await walkRepository.CreateAsync(walkDomainModel);

                // Map Domain Model to DTO
                return Ok(mapper.Map<WalkDto>(walkDomainModel));
            
         
        }
        // GET Walks
        // GET: /api/walks?filterOn=name&filterQuery=Track&sortBy=Name&isAscending=true
        [HttpGet]
        public async Task<IActionResult> GetAll([FromQuery]string? filterOn,
            [FromQuery] string? filterQuery,
            [FromQuery] string? sortBy,
            [FromQuery] bool? isAscending, 
            [FromQuery] int pageNumber=1, 
            [FromQuery] int pageSize=1000) 
        {
         var WalksDomainModel = await  walkRepository.GetAllAsync(filterOn, filterQuery,sortBy,isAscending?? true,
             pageNumber,pageSize);
            // Map Domain Model to DTO
            return Ok( mapper.Map<List<WalkDto>>(WalksDomainModel));
        }
        [HttpGet]
        [Route("{id:Guid}")]
        public async Task<IActionResult> GetById([FromRoute] Guid id) {
        var walkDomainModel=await walkRepository.GetByIdAsync(id);
            if (walkDomainModel == null) {
            return NotFound();
            }
         return Ok(  mapper.Map<WalkDto>(walkDomainModel));
        }
        // Update Walk By Id
        // PUT: api/Walks/{id}
        [HttpPut]
        [Route("{id:Guid}")]
        [ValidateModel]
        public async Task<IActionResult> Update([FromRoute] Guid id, UpdateWalkRequestDto updateWalkRequestDto) 
        {
                // Map DTO to Domain Model
                var walkDomainModel = mapper.Map<Walk>(updateWalkRequestDto);

                walkDomainModel = await walkRepository.UpdateAsync(id, walkDomainModel);

                if (walkDomainModel == null)
                {
                    return NotFound();
                }
                return Ok(mapper.Map<WalkDto>(walkDomainModel));
            }
        //Delete a Walk By Id
        // DELETE: /api/Walks/{id}
        [HttpDelete]
        public async Task<IActionResult> Delete([FromRoute] Guid id)
        {
            var deleteEelement = await walkRepository.DeleteAsync(id);

            if (deleteEelement == null)
            {
                return NotFound();
            }
            // Map Domain Model to DTO
            return Ok(mapper.Map<WalkDto>(deleteEelement));
        }
    }
       
    }

