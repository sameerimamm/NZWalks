using Microsoft.EntityFrameworkCore;
using NZWalks.API.Data;
using NZWalks.API.Models.Domain;
using NZWalks.API.Models.DTO;

namespace NZWalks.API.Repositories
{


    public class SQLRegionRepository : IRegionRepository
    {
        private readonly NZWalksDbContext dbContext;

        public SQLRegionRepository(NZWalksDbContext dbContext)
        { 
            this.dbContext = dbContext;
        }

        public async Task<Region> AddAsync(Region region)
        {
            await dbContext.Regions.AddAsync(region);
            await dbContext.SaveChangesAsync();

            return region;
        }

        public async Task<Region?> DeleteAsync(Guid id)
        {
            var region = await dbContext.Regions.FindAsync(id);
            if (region == null)
            {
                return null;
            }

            dbContext.Regions.Remove(region);
            await dbContext.SaveChangesAsync();

            return region;



        }


        public async Task<List<Region>> GetAllAsync()
        {
            var Regions = await dbContext.Regions.ToListAsync();
            return Regions;
            //return  await dbContext.Regions.ToListAsync();
        }

        public async Task<Region?> GetByIdAsync(Guid id)
        {
            var region = await dbContext.Regions.FindAsync(id);

            return region;

        }

        public async Task<Region?> UpdateAsync(Guid id , AddRegionDto regionDto)

        {

            var tobe_updated_region = await dbContext.Regions.FindAsync(id);
            if (tobe_updated_region == null)
            {
                return null;
            }
            tobe_updated_region.Code = regionDto.Code;
            tobe_updated_region.Name = regionDto.Name;
            tobe_updated_region.RegionImageUrl = regionDto.RegionImageUrl;
            await dbContext.SaveChangesAsync();

            return tobe_updated_region;
        }

    }
}
