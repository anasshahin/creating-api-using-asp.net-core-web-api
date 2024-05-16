using Microsoft.EntityFrameworkCore;
using NZWalksAPI.Data;
using NZWalksAPI.Models.Domain;
using System.Linq;

namespace NZWalksAPI.Repositories
{
    public class SQLWalkRepository : IWalkRepository
    {
        private readonly NZWalkDbContext dbContext;

        public SQLWalkRepository(NZWalkDbContext dbContext)
        {
            this.dbContext = dbContext;
        }
        public async Task<Walk> CreateAsync(Walk walk)
        {
           await dbContext.Walks.AddAsync(walk); 
           await dbContext.SaveChangesAsync(); 
            return walk;
        }

        public async Task<Walk?> DeleteAsync(Guid id)
        {
         var existingWalk=  await dbContext.Walks.FirstOrDefaultAsync(x => x.Id == id);
            if (existingWalk == null) 
            {
                return null;
            }
            dbContext.Walks.Remove(existingWalk);
            await dbContext.SaveChangesAsync();
            return existingWalk;
        }

        public async Task<List<Walk>> GetAllAsync(string? filterOn=null, string? filterQuery = null,
            string? sortBy = null,
             bool isAscending = true,
             int pageNumber = 1, int pageSize = 1000)
        {
            var wlaks =  dbContext.Walks.Include("Region").Include("Difficulty").AsQueryable();

            // Filtering
            if (string.IsNullOrWhiteSpace(filterOn) == false && string.IsNullOrWhiteSpace(filterQuery) == false)
            {
                if (filterOn.Equals("name",StringComparison.OrdinalIgnoreCase))
                wlaks = wlaks.Where(x=> x.Name.Contains(filterQuery));
            }
            // Sorting
            if (string.IsNullOrWhiteSpace(sortBy) == false) 
            {
                if (sortBy.Equals("Name", StringComparison.OrdinalIgnoreCase))
                {
                    wlaks = isAscending ? wlaks.OrderBy(x => x.Name) : wlaks.OrderByDescending(x => x.Name);
                }
                else if (sortBy.Equals("Length",StringComparison.OrdinalIgnoreCase)) 
                {
                    wlaks = isAscending ? wlaks.OrderBy(x => x.LengthInKm) : wlaks.OrderByDescending(x=>x.LengthInKm);
                }
            }
            // Pagination
            var skipResults = (pageNumber - 1) * pageSize;
            
                return await wlaks.Skip(skipResults).Take(pageSize).ToListAsync();
              //  return await wlaks.ToListAsync();
            //  return await dbContext.Walks.Include("Region").Include("Difficulty").ToListAsync();


        }

        public async Task<Walk?> GetByIdAsync(Guid id)
        {
         return  await dbContext.Walks.Include("Difficulty").
                Include("Region").
                FirstOrDefaultAsync(x=> x.Id == id);
        }

        public async Task<Walk?> UpdateAsync(Guid id, Walk walk)
        {
          var existingWalk=  await dbContext.Walks.FirstOrDefaultAsync(x=>x.Id == id);

            if (existingWalk == null) 
            {
                return null;
            }
           existingWalk.Name = walk.Name;
            existingWalk.Description= walk.Description;
            existingWalk.LengthInKm = walk.LengthInKm;
            existingWalk.RegionId = walk.RegionId;  
            existingWalk.DifficultyId = walk.DifficultyId;
            existingWalk.WalkImageUrl = walk.WalkImageUrl;
           await dbContext.SaveChangesAsync();
            return existingWalk;
        }
    }
}
