using Domain.Entities;

namespace ApplicationCore.Services
{
    public interface IClientServices
    {
        Task<IReadOnlyList<Client>> GetAllAsync();
        Task<Client?> GetByIdAsync(int id);
        Task<Client> AddAsync(Client entity);
        Task<Client?> UpdateAsync(int id, Client entity, List<string> fieldsToUpdate);
        Task<bool> DeleteAsync(int id);
    }
}
