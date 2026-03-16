using Domain.Entities;

namespace ApplicationCore.Services
{
    public interface ILitMedicalServices
    {
        Task<IReadOnlyList<LitMedical>> GetAllAsync(bool? disponible = null);
        Task<LitMedical?> GetByIdAsync(int id);
        Task<LitMedical> AddAsync(LitMedical entity);
        Task<LitMedical?> UpdateAsync(int id, LitMedical entity, List<string> fieldsToUpdate);
        Task<bool> DeleteAsync(int id);
    }
}
