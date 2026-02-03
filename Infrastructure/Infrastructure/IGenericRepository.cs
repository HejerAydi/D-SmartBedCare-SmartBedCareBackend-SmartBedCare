using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Infrastructure
{
    public interface IGenericRepository<T> where T : class
    {
        Task<IQueryable<T>> GetAllAsyncwithfilter(
       Expression<Func<T, bool>>? filter = null,
       Func<IQueryable<T>, IOrderedQueryable<T>>? orderBy = null, bool Notransaction = false);
        Task<T?> GetByIdAsync(int id);
        Task AddAsync(T entity);
        Task UpdateAsync(T entity);
        Task DeleteAsync(T entity);
        Task SaveChangesAsync();
        Task<T> GetAsync(Expression<Func<T, bool>> predicate);
        T Get(Expression<Func<T, bool>> predicate);
        Task<T?> GetByIdAsync(params object[] keyValues);
        Task AddListAsync(IEnumerable<T> entities);
        Task DeleteListAsync(IEnumerable<T> entities);

        Task<string> CreateAndLogAsync(T entity);


        Task<string> UpdateGeneral(T source, T dest, List<string> champamodifier = null);
    }

}
