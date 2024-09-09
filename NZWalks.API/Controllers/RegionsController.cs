using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NZWalks.API.Data;
using NZWalks.API.Models.Domain;
using NZWalks.API.Models.DTO;
using NZWalks.API.Repositories;
using System;
using System.Data;
using System.Text.Json;

namespace NZWalks.API.Controllers
{
    // https://localhost:portnumber/api/regions
    // [Route("api/[controller]")]
    [Route("api/regions")]
    [ApiController]

    public class RegionsController : ControllerBase
    {
        private readonly NZWalksDbContext dbContext;
        private readonly IRegionRepository regionRepository;
        private readonly IMapper mapper;
        private readonly ILogger<RegionsController> logger;

        public RegionsController(NZWalksDbContext dbContext,
            IRegionRepository regionRepository, IMapper mapper,
            ILogger<RegionsController> logger)
        {
            // this.dbContext = dbContext;
            this.dbContext = dbContext;
            this.regionRepository = regionRepository;
            this.mapper = mapper;
            this.logger = logger;
        }

        [HttpGet]
        //[Authorize]

        public async Task<IActionResult> GetAll()
        {
            logger.LogInformation("GetAll Region action method was invoked");
            //     var regionsoff = new List<Region> {
            //     new Region{  Id = Guid.NewGuid(), } };

            // var regions = dbContext.Regions.ToList();


            // var regions = await dbContext.Regions.ToListAsync();

            var regions = await regionRepository.GetAllAsync();
            var regionsDto = mapper.Map<List<RegionDto>>(regions);

            //var regionsDto = new List<RegionDto>();
            //foreach (var region in regions)
            //{
            //    regionsDto.Add(new RegionDto()
            //    {
            //        Id = region.Id,
            //        Name = region.Name,
            //        Code = region.Code,
            //        RegionImageUrl = region.RegionImageUrl
            //    });

            //}

            logger.LogInformation($"Finished getall data providing");

            foreach (var region in regionsDto) { 
                logger.LogInformation(JsonSerializer.Serialize(region));
            }




            return Ok(regionsDto);

        }


        [HttpGet("{id:Guid}")]
        //[Authorize(Roles = "Reader, Writer")]

        public async Task<IActionResult> GetById(Guid id)
            {
                //var region = dbContext.Regions.FirstOrDefault(u => u.Id == id);
                //var region =await dbContext.Regions.FindAsync(id);

                //var region =await dbContext.Regions.FindAsync(id);
                var region =await regionRepository.GetByIdAsync(id);


                if (region == null) {
                    return NotFound();
                }
            //var regionDto = new List<RegionDto>();

            //regionDto.Add(new RegionDto()
            //{
            //    Id = region.Id,
            //    Name = region.Name,
            //    Code = region.Code,
            //    RegionImageUrl = region.RegionImageUrl

            //});
            var regionDto = mapper.Map<RegionDto>(region);

            return Ok(regionDto);
            }

        [HttpPost]
        //[Authorize(Roles = "Writer")]
        public async Task<IActionResult> AddRegion([FromBody] AddRegionDto addNewRegion)
        {
            Console.WriteLine(ModelState.IsValid);
            Console.WriteLine("Sameer");
            Console.WriteLine(!ModelState.IsValid);

            if (ModelState.IsValid)
            {
                Console.WriteLine("Gandi baat");


                var regionAdd = new Region
                {
                    Code = addNewRegion.Code,
                    Name = addNewRegion.Name,
                    RegionImageUrl = addNewRegion.RegionImageUrl
                };


                Console.WriteLine(regionAdd.Id + "Pehla");


                await regionRepository.AddAsync(regionAdd);


                var createdRegion = new RegionDto
                {
                    Id = regionAdd.Id,
                    Code = addNewRegion.Code,
                    Name = regionAdd.Name,
                    RegionImageUrl = regionAdd.RegionImageUrl
                };


                Console.WriteLine(regionAdd.Id);


                return CreatedAtAction(nameof(GetById), new { id = regionAdd.Id }, createdRegion);

            }
            else
            {
                return NotFound(ModelState);
            }

          //  return CreatedAtRoute(nameof(GetById), new { id = regionAdd.Id }, addNewRegion);


        }

        [HttpPut("{id:Guid}")]
        //[Authorize(Roles = "Writer")]
        public async  Task<IActionResult> UpdateById([FromBody] AddRegionDto regionDto, Guid id)
        {
            if (!ModelState.IsValid)
            {
                Console.WriteLine("Hello");
                return BadRequest(ModelState);
            }

            //var region = await dbContext.Regions.FindAsync(id);
            var region= await regionRepository.UpdateAsync(id, regionDto);


            if (region == null)
            {
                return NotFound();
            }

            //region.Code = regionDto.Code;
            //region.Name = regionDto.Name;
            //region.RegionImageUrl = regionDto.RegionImageUrl;
            //await dbContext.SaveChangesAsync(); 

            var UpdatedRegion = new RegionDto
            {
                Id = region.Id,
                Name = region.Name,
                RegionImageUrl = region.RegionImageUrl,
                Code = region.Code

            };

            return Ok(UpdatedRegion);


        }


        [HttpDelete("{id:Guid}")]
        //[Authorize(Roles = "Writer")]
        public async Task<IActionResult> Delete( Guid id)
        {
            //var region = await dbContext.Regions.FindAsync(id);

            var region = await regionRepository.DeleteAsync(id);

            if (region == null)
            {
                return NotFound();
            }


            //dbContext.Regions.Remove(region);
            //await dbContext.SaveChangesAsync();

            var deletedRegion = new RegionDto
            {
                Id = region.Id,
                Name = region.Name,
                RegionImageUrl = region.RegionImageUrl,
                Code = region.Code
            };


            return Ok(deletedRegion);



        }



        }
    }
    
