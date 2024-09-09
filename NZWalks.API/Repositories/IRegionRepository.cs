using NZWalks.API.Models.Domain;
using NZWalks.API.Models.DTO;
using System.Runtime.InteropServices;

namespace NZWalks.API.Repositories
{
    public interface IRegionRepository
    {
        Task<List<Region>> GetAllAsync();
        Task<Region?> GetByIdAsync(Guid id);
        
        Task<Region> AddAsync(Region region);

        Task<Region?> UpdateAsync( Guid id, AddRegionDto region);
        Task<Region?> DeleteAsync(Guid id);

    }
    



}
