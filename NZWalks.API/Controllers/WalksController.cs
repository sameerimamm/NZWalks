using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NZWalks.API.Models.Domain;
using NZWalks.API.Models.DTO;
using NZWalks.API.Repositories;

namespace NZWalks.API.Controllers
{
    // api/walks
    [Route("api/[controller]")]
    [ApiController]
    public class WalksController : ControllerBase
    {
        private IMapper mapper;
        private readonly IWalkRepository walkRepository;

        public WalksController(IMapper mapper, IWalkRepository walkRepository)
        {
            this.mapper = mapper;
            this.walkRepository = walkRepository;
        }


        //CREATE WALK

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] WalkUpdateDto addWalkDto)
        {
            var walk = mapper.Map<Walk>(addWalkDto);

            await walkRepository.CreateAsync(walk);

            var created_walk = mapper.Map<WalkDto>(walk);

            return Ok(created_walk);


        }

        // Get Walks
        // GET: /api/walks?filterOn=Name&filterQuery=Track&sortBy=Name&isAscending=true&pageNumber=1&pageSize=10
        [HttpGet]
        public async Task<IActionResult> GetAll([FromQuery] string? filterOn,[FromQuery] string? filterQuery,
            [FromQuery] string? sortBy, [FromQuery] bool isAscending, 
            [FromQuery] int pageNumber = 1, [FromQuery]int pageSize=1000)
        {
            var walks =  await walkRepository.GetAllAsync(filterOn,filterQuery,
                sortBy,isAscending,pageNumber,pageSize);

            var walksDto = mapper.Map<List<WalkDto>>(walks);

            return Ok(walksDto);
        }

        // Get walk by id
        [HttpGet("{id:Guid}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            var walk = await walkRepository.GetByIdAsync(id);

            if (walk == null)
            {
                return NotFound();
            }
            var walkDto = mapper.Map<WalkDto>(walk);
            return Ok(walkDto);
        }
        [HttpPut("{id:Guid}")]
        public async Task<IActionResult> UpdateWalk([FromBody] WalkUpdateDto walkUpdateDto,  Guid id)
        {
            var walk = await walkRepository.WalkUpdateAsync(id, walkUpdateDto);

            var walkDto = mapper.Map<WalkDto>(walk);

            return Ok(walkDto);
        }

        [HttpDelete("{id:Guid}")]
        public async Task<IActionResult> DeleteWalk(Guid id)
        {
            var walk = await walkRepository.DeleteWalkAsync(id);
            if (walk == null)
            {
                return BadRequest();
            }

            var walkDto =mapper.Map<WalkDto>(walk);
            return Ok(walkDto);

        }

    }
}
