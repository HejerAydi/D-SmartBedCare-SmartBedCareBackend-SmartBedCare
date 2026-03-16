using Domain.Entities;
using Infrastructure.Infrastructure;

namespace ApplicationCore.Services
{
    public class PaiementServices : IPaiementServices
    {
        private readonly IUnitOfWork _unitOfWork;

        public PaiementServices(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
        }

        public async Task<IReadOnlyList<Paiement>> GetByLocationAsync(int locationId)
        {
            try
            {
                var result = await _unitOfWork.Repository<Paiement>().GetAllAsyncwithfilter(p => p.LocationId == locationId);
                return result.ToList();
            }
            catch (Exception ex) { throw genException.GenericException.GenException(ex, _unitOfWork); }
        }

        public async Task<Paiement> AddAsync(Paiement entity)
        {
            try
            {
                var location = await _unitOfWork.Repository<Location>().GetByIdAsync(entity.LocationId);
                if (location == null)
                    throw new Exception("Erreur : Location introuvable.");

                entity.DatePaiement = DateTime.Now;
                await _unitOfWork.Repository<Paiement>().AddAsync(entity);

                // Historique
                var historique = new Historique
                {
                    TableName = "Paiement",
                    Action = "CREATE",
                    RecordId = entity.LocationId,
                    Description = $"Paiement de {entity.Montant} DT ajouté - Mode: {entity.ModePaiement}",
                    DateAction = DateTime.Now
                };
                await _unitOfWork.Repository<Historique>().AddAsync(historique);

                return entity;
            }
            catch (Exception ex) { throw genException.GenericException.GenException(ex, _unitOfWork); }
        }
    }
}
