using Domain.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ApplicationCore.Services
{
    public interface ITestServices
    {
        Task<IReadOnlyList<Test>> GetAllAsync();
        Task<Test?> GetByIdAsync(int id);
        Task AddAsync(Test entity);
        Task<Test?> UpdateAsync(int id, Test entity, List<string> fieldsToUpdate);
        Task<bool> Delete(int id);
    }
}