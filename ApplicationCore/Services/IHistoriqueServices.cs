using Domain.Entities;

namespace ApplicationCore.Services
{
    public interface IHistoriqueServices
    {
        Task<IReadOnlyList<Historique>> GetAllAsync();
        Task<IReadOnlyList<Historique>> GetByTableAsync(string tableName);
    }
}
