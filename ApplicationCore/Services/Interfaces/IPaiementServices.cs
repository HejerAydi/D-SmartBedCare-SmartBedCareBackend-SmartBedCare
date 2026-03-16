using Domain.Entities;

namespace ApplicationCore.Services
{
    public interface IPaiementServices
    {
        Task<IReadOnlyList<Paiement>> GetByLocationAsync(int locationId);
        Task<Paiement> AddAsync(Paiement entity);
    }
}
