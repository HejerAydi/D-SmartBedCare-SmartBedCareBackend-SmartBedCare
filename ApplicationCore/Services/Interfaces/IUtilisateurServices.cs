using Domain.Entities;

namespace ApplicationCore.Services
{
    public interface IUtilisateurServices
    {
        Task<IReadOnlyList<Utilisateur>> GetAllAsync();
        Task<Utilisateur?> GetByIdAsync(int id);
        Task<Utilisateur> AddAsync(Utilisateur entity);
        Task<Utilisateur?> UpdateAsync(int id, Utilisateur entity, List<string> fieldsToUpdate);
        Task<bool> DeleteAsync(int id);
        Task<bool> ToggleActiveAsync(int id);
        Task<Utilisateur?> GetByEmailAsync(string email);
    }
}
