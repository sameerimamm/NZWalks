using NZWalks.API.Models.Domain;

namespace NZWalks.API.Repositories
{
    public interface IWalkRepository
    {
       Task<Walk> CreateAsync(Walk walk);
       Task<List<Walk>>GetAllAsync(string? filerOn=null, string? filterQuery=null, 
           string? sortBy=null, bool isAscending=true,
           int pageNumber=1, int pageSize=1000);
       Task<Walk?> GetByIdAsync(Guid id);
       Task<Walk?> WalkUpdateAsync(Guid id, Models.DTO.WalkUpdateDto walkUpdateDto);
       Task<Walk?> DeleteWalkAsync(Guid id);

    }
}
