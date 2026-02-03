
using Infrastructure.Data;
using Infrastructure.Infrastructure;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Repositories
{
    public class GenericRepository<T> : IGenericRepository<T> where T : class
    {
        private readonly AppDbContext _context;
        private readonly DbSet<T> _dbSet;

        public GenericRepository(AppDbContext context)
        {
            _context = context;
            _dbSet = _context.Set<T>();
        }


        public async Task<IQueryable<T>> GetAllAsyncwithfilter(
         Expression<Func<T, bool>>? filter = null,
         Func<IQueryable<T>, IOrderedQueryable<T>>? orderBy = null, bool Notransaction = false)
        {
            try
            {
                IQueryable<T> query = _dbSet;

                // Applique le filtre si fourni
                if (Notransaction)
                    query.AsNoTracking();
                if (filter != null)
                {
                    query = query.Where(filter);
                }

                // Applique l'orderBy (tri) s'il est fourni, il est toujours appliqué
                if (orderBy != null)
                {
                    query = orderBy(query);
                }

                // Retourne la requête sans l'exécuter encore (paresseux)
                return await Task.FromResult(query);
            }
            catch (Exception ex)
            {
                throw new Exception($"Erreur 0702025 0925 : Erreur lors de la récupération de tous les éléments : {ex.Message}", ex);
            }
        }

        public async Task<T> GetAsync(Expression<Func<T, bool>> predicate)
        {
            return await _dbSet.FirstOrDefaultAsync(predicate);
        }

        public async Task<T?> GetByIdAsync(int id)
        {
            try
            {
                return await _dbSet.FindAsync(id);
            }
            catch (Exception ex)
            {
                throw new Exception($"Erreur 0702025 0925 :Erreur lors de la récupération de l'élément avec ID {id} : {ex.Message}", ex);
            }

        }
        public async Task<T?> GetByIdAsync(params object[] keyValues)
        {
            try
            {
                return await _dbSet.FindAsync(keyValues);
            }
            catch (Exception ex)
            {
                throw new Exception($"Erreur lors de la récupération de l'élément avec ID(s) {string.Join(", ", keyValues)} : {ex.Message}", ex);
            }
        }

        public async Task AddAsync(T entity)
        {

            try
            {
                await _dbSet.AddAsync(entity);
                //       _context.SaveChanges();
                //t3wdh commit mt3 unitofwork
            }
            catch (Exception ex)
            {
                throw new Exception($"Erreur 0702025 0928 :Erreur lors de l'ajout de l'élément : {ex.Message}", ex);
            }
        }

        //createwithstringlog
        public async Task<string> CreateAndLogAsync(T entity)
        {
            try
            {
                var properties = typeof(T).GetProperties();
                var defaultInstance = Activator.CreateInstance<T>(); // Create a default (empty) object

                List<string> fieldValues = new List<string>();

                foreach (var property in properties)
                {
                    var value = property.GetValue(entity);
                    var defaultValue = property.GetValue(defaultInstance);

                    // Log only if value is not null AND different from the default (i.e., user set it)
                    if (value != null && !value.Equals(defaultValue))
                    {
                        fieldValues.Add($"{property.Name}={value}");
                    }
                }

                string logMessage = $"Created {typeof(T).Name} with " + string.Join(", ", fieldValues);
                return logMessage;
            }
            catch (Exception ex)
            {
                throw new Exception($"Error creating entity {typeof(T).Name}: {ex.Message}", ex);
            }
        }

        public async Task UpdateAsync(T entity)
        {
            try
            {
                _dbSet.Update(entity);
            }
            catch (Exception ex)
            {
                throw new Exception($"Erreur 0702025 0928 :Erreur lors de la mise à jour de l'élément : {ex.Message}", ex);
            }
        }

        public async Task DeleteAsync(T entity)
        {
            try
            {
                _dbSet.Remove(entity);
            }
            catch (Exception ex)
            {
                throw new Exception($"Erreur 0702025 0928 :Erreur lors de la suppression de l'élément : {ex.Message}", ex);
            }
        }

        public async Task SaveChangesAsync()
        {
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                var inner = ex.InnerException?.Message ?? "No inner exception";
                throw new Exception($"Erreur 0702025 0928 : {ex.Message} | Inner: {inner}", ex);
            }
        }


        public async Task AddListAsync(IEnumerable<T> entities)
        {
            try
            {
                await _dbSet.AddRangeAsync(entities);
            }
            catch (Exception ex)
            {
                throw new Exception($"Erreur 0702025 0930 : Erreur lors de l'ajout de plusieurs éléments : {ex.Message}", ex);
            }
        }

        public T Get(Expression<Func<T, bool>> predicate)
        {
            try
            {
                return _dbSet.FirstOrDefault(predicate);
            }
            catch (Exception ex)
            {
                throw new Exception($"Erreur lors de la récupération de l'élément : {ex.Message}", ex);
            }
        }


        public async Task DeleteListAsync(IEnumerable<T> entities)
        {
            try
            {
                _dbSet.RemoveRange(entities);
            }
            catch (Exception ex)
            {
                throw new Exception($"Erreur 0702025 0932 : Erreur lors de la suppression de plusieurs éléments : {ex.Message}", ex);
            }
        }

        public async Task<string> UpdateGeneral(T source, T dest, List<string> champamodifier = null)
        {
            try
            {
                var properties = typeof(T).GetProperties();
                List<string> changes = new List<string>();

                foreach (var property in properties)
                {
                    if (champamodifier != null && !champamodifier.Contains(property.Name))
                        continue;

                    var sourceValue = property.GetValue(source);
                    var destValue = property.GetValue(dest);

                    if (!Equals(sourceValue, destValue) && destValue != null)
                    {
                        // Track the change
                        changes.Add($"Changed {property.Name} from '{sourceValue}' to '{destValue}'");

                        // Update the source property with the value from the destination
                        property.SetValue(source, destValue);
                    }
                }

                await _context.SaveChangesAsync();

                return changes.Count > 0 ? string.Join(", ", changes) : "No changes made.";
            }
            catch (Exception ex)
            {
                throw new Exception($"Erreur update : {ex.Message}", ex);
            }




        }
    }
}
