using Domain.DTOs;
using Domain.Entities;

namespace ApplicationCore.Services
{
    public interface ILocationServices
    {
        Task<IReadOnlyList<Location>> GetAllAsync();
        Task<Location?> GetByIdAsync(int id);
        Task<Location> AddAsync(CreateLocationDTO dto, int createdBy);
        Task<Location?> UpdateAsync(int id, Location entity, List<string> fieldsToUpdate);
        Task<bool> RecupererAsync(int id);
    }
}
