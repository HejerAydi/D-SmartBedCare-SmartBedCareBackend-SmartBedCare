using Domain.Entities;
using Infrastructure.Infrastructure;

namespace ApplicationCore.Services
{
    public class HistoriqueServices : IHistoriqueServices
    {
        private readonly IUnitOfWork _unitOfWork;

        public HistoriqueServices(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
        }

        public async Task<IReadOnlyList<Historique>> GetAllAsync()
        {
            try
            {
                var result = await _unitOfWork.Repository<Historique>().GetAllAsyncwithfilter(
                    orderBy: q => q.OrderByDescending(h => h.DateAction));
                return result.ToList();
            }
            catch (Exception ex) { throw genException.GenericException.GenException(ex, _unitOfWork); }
        }

        public async Task<IReadOnlyList<Historique>> GetByTableAsync(string tableName)
        {
            try
            {
                var result = await _unitOfWork.Repository<Historique>().GetAllAsyncwithfilter(
                    filter: h => h.TableName == tableName,
                    orderBy: q => q.OrderByDescending(h => h.DateAction));
                return result.ToList();
            }
            catch (Exception ex) { throw genException.GenericException.GenException(ex, _unitOfWork); }
        }
    }
}
