using Domain.DTOs;
using Domain.Entities;
using Infrastructure.Infrastructure;
using Microsoft.EntityFrameworkCore;

namespace ApplicationCore.Services
{
    public class LocationServices : ILocationServices
    {
        private readonly IUnitOfWork _unitOfWork;

        public LocationServices(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
        }

        public async Task<IReadOnlyList<Location>> GetAllAsync()
        {
            try
            {
                var result = await _unitOfWork.Repository<Location>().GetAllAsyncwithfilter();
                return await result
                    .Include(l => l.Client)
                    .Include(l => l.LocationLits).ThenInclude(ll => ll.LitMedical)
                    .Include(l => l.LocationRubriques).ThenInclude(lr => lr.Rubrique)
                    .Include(l => l.Paiements)
                    .ToListAsync();
            }
            catch (Exception ex) { throw genException.GenericException.GenException(ex, _unitOfWork); }
        }

        public async Task<Location?> GetByIdAsync(int id)
        {
            try
            {
                var result = await _unitOfWork.Repository<Location>().GetAllAsyncwithfilter(l => l.Id == id);
                return await result
                    .Include(l => l.Client)
                    .Include(l => l.LocationLits).ThenInclude(ll => ll.LitMedical)
                    .Include(l => l.LocationRubriques).ThenInclude(lr => lr.Rubrique)
                    .Include(l => l.Paiements)
                    .FirstOrDefaultAsync();
            }
            catch (Exception ex) { throw genException.GenericException.GenException(ex, _unitOfWork); }
        }

        public async Task<Location> AddAsync(CreateLocationDTO dto, int createdBy)
        {
            try
            {
                // Vérifier client
                var client = await _unitOfWork.Repository<Client>().GetByIdAsync(dto.ClientId);
                if (client == null)
                    throw new Exception("Erreur : Client introuvable.");

                // Vérifier lits disponibles
                var lits = new List<LitMedical>();
                foreach (var litId in dto.LitIds)
                {
                    var lit = await _unitOfWork.Repository<LitMedical>().GetByIdAsync(litId);
                    if (lit == null) throw new Exception($"Erreur : Lit ID {litId} introuvable.");
                    if (!lit.Disponible) throw new Exception($"Erreur : Lit {lit.NumeroSerie} n'est pas disponible.");
                    lits.Add(lit);
                }

                // Calculer montant total
                decimal montantTotal = lits.Sum(l => l.PrixLocation + l.FraisTransport);

                var location = new Location
                {
                    ClientId = dto.ClientId,
                    DateLocation = DateTime.Now,
                    DateLivraison = dto.DateLivraison,
                    DateRecuperation = dto.DateRecuperation,
                    Statut = "EnCours",
                    MontantTotal = montantTotal,
                    CreatedBy = createdBy
                };

                await _unitOfWork.Repository<Location>().AddAsync(location);
                _unitOfWork.Save();

                // Ajouter LocationLits + marquer lits indisponibles
                foreach (var lit in lits)
                {
                    var locationLit = new LocationLit
                    {
                        LocationId = location.Id,
                        LitId = lit.Id,
                        PrixLocation = lit.PrixLocation,
                        FraisTransport = lit.FraisTransport
                    };
                    await _unitOfWork.Repository<LocationLit>().AddAsync(locationLit);
                    lit.Disponible = false;
                    await _unitOfWork.Repository<LitMedical>().UpdateAsync(lit);
                }

                // Ajouter rubriques
                foreach (var r in dto.Rubriques)
                {
                    var lr = new LocationRubrique
                    {
                        LocationId = location.Id,
                        RubriqueId = r.RubriqueId,
                        Description = r.Description
                    };
                    await _unitOfWork.Repository<LocationRubrique>().AddAsync(lr);
                }

                // Créer notification rappel récupération (3 jours avant)
                var notification = new Notification
                {
                    LocationId = location.Id,
                    Message = $"Rappel : récupération du lit prévue le {dto.DateRecuperation:dd/MM/yyyy}",
                    DateNotification = dto.DateRecuperation.AddDays(-3),
                    Type = "RappelRecuperation",
                    IsRead = false
                };
                await _unitOfWork.Repository<Notification>().AddAsync(notification);

                // Historique
                var historique = new Historique
                {
                    TableName = "Location",
                    Action = "CREATE",
                    RecordId = location.Id,
                    Description = $"Location créée pour client ID {dto.ClientId}",
                    UtilisateurId = createdBy,
                    DateAction = DateTime.Now
                };
                await _unitOfWork.Repository<Historique>().AddAsync(historique);

                return location;
            }
            catch (Exception ex) { throw genException.GenericException.GenException(ex, _unitOfWork); }
        }

        public async Task<Location?> UpdateAsync(int id, Location entity, List<string> fieldsToUpdate)
        {
            try
            {
                var existing = await _unitOfWork.Repository<Location>().GetByIdAsync(id);
                if (existing == null)
                    throw new Exception($"Erreur : Location ID {id} introuvable.");

                await _unitOfWork.Repository<Location>().UpdateGeneral(existing, entity, fieldsToUpdate);
                return existing;
            }
            catch (Exception ex) { throw genException.GenericException.GenException(ex, _unitOfWork); }
        }

        public async Task<bool> RecupererAsync(int id)
        {
            try
            {
                var location = await _unitOfWork.Repository<Location>().GetByIdAsync(id);
                if (location == null)
                    throw new Exception($"Erreur : Location ID {id} introuvable.");

                location.Statut = "Terminé";
                await _unitOfWork.Repository<Location>().UpdateAsync(location);

                // Remettre les lits disponibles
                var locationLits = await _unitOfWork.Repository<LocationLit>().GetAllAsyncwithfilter(ll => ll.LocationId == id);
                foreach (var ll in locationLits)
                {
                    var lit = await _unitOfWork.Repository<LitMedical>().GetByIdAsync(ll.LitId);
                    if (lit != null)
                    {
                        lit.Disponible = true;
                        await _unitOfWork.Repository<LitMedical>().UpdateAsync(lit);
                    }
                }

                return true;
            }
            catch (Exception ex) { throw genException.GenericException.GenException(ex, _unitOfWork); }
        }
    }
}
