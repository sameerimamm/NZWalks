using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Client;
using NZWalks.API.Data;
using NZWalks.API.Models.Domain;
using NZWalks.API.Models.DTO;
using System.Globalization;

namespace NZWalks.API.Repositories
{
   
    public class SQLWalkRepository : IWalkRepository

    {
        private readonly NZWalksDbContext dbContext;

        public SQLWalkRepository(NZWalksDbContext dbContext)
        {
            this.dbContext = dbContext;
        }
        public async Task<Walk> CreateAsync(Walk walk)
        {
            await dbContext.Walks.AddAsync(walk);
            await dbContext.SaveChangesAsync();
            return walk;
            
        }

        public async Task<Walk?> DeleteWalkAsync(Guid id)
        {
            var walk = await dbContext.Walks.FindAsync(id);
            if (walk == null)
            {
                return null;
            }
            dbContext.Walks.Remove(walk);
            await dbContext.SaveChangesAsync();
            return walk;

        }

        public async Task<List<Walk>> GetAllAsync(string? filterOn = null, string? filterQuery = null,
           string?  sortBy= null, bool isAscending = true,
            int pageNumber = 1, int pageSize = 1000)
        {
            var walks =  dbContext.Walks.Include("Difficulty").Include("Region").AsQueryable();
            //filtering
            if (string.IsNullOrWhiteSpace(filterOn) == false && string.IsNullOrWhiteSpace(filterQuery) == false)
            {
   
                if(filterOn.Equals(filterOn, StringComparison.OrdinalIgnoreCase))
                {
                    walks = walks.Where(x => x.Name.Contains(filterQuery));

                }

                if (filterOn.Equals("Description", StringComparison.OrdinalIgnoreCase))
                {
                    walks = walks.Where(x => x.Description.Contains(filterQuery));

                }

            }

            //sorting
            if (string.IsNullOrWhiteSpace(sortBy) == false)
            {
                if (sortBy.Equals("name", StringComparison.OrdinalIgnoreCase))
                {

                    walks = isAscending ? walks.OrderBy(x => x.Name) : walks.OrderByDescending(x => x.Name);

                }
                if (sortBy.Equals("length", StringComparison.OrdinalIgnoreCase))
                {

                    if (isAscending == true)
                    {
                        walks = walks.OrderBy(x => x.LengthInKm);
                    }

                    else
                    {
                        walks = walks.OrderByDescending(x => x.LengthInKm);
                    }

                }
            }
            //pagination
            var skipResult = (pageNumber - 1) * pageSize;


            return await walks.Skip(skipResult).Take(pageSize).ToListAsync();

            //return await dbContext.Walks.Include("Difficulty").Include("Region").ToListAsync();
        }

        public async Task<Walk?> GetByIdAsync(Guid id)
        {
            var walk = await dbContext.Walks.Include("Difficulty").Include("Region").FirstOrDefaultAsync(i => i.Id == id);
            if (walk == null)
            {
                return null;
            }
            return walk;
        }

        public async Task<Walk?> WalkUpdateAsync(Guid id ,WalkUpdateDto walkDto)
        {
            var walk = await dbContext.Walks.Include("Difficulty").Include("Region").FirstOrDefaultAsync(i => i.Id == id);

            if (walk == null)
            {
                return null;
            }

            Console.WriteLine(walk.DifficultyId);

            walk.Name = walkDto.Name;
            walk.Description= walkDto.Description;
            walk.LengthInKm=walkDto.LengthInKm;
            walk.DifficultyId = walkDto.DifficultyId;
            walk.RegionId=walkDto.RegionId; 
            walk.WalkImageUrl = walkDto.WalkImageUrl;

            await dbContext.SaveChangesAsync();

            Console.WriteLine(walk.DifficultyId);




            return walk;
        }
    }
}
