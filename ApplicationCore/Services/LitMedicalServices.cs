using Domain.Entities;
using Infrastructure.Infrastructure;

namespace ApplicationCore.Services
{
    public class LitMedicalServices : ILitMedicalServices
    {
        private readonly IUnitOfWork _unitOfWork;

        public LitMedicalServices(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
        }

        public async Task<IReadOnlyList<LitMedical>> GetAllAsync(bool? disponible = null)
        {
            try
            {
                var result = await _unitOfWork.Repository<LitMedical>().GetAllAsyncwithfilter(
                    disponible.HasValue ? l => l.Disponible == disponible.Value : null);
                return result.ToList();
            }
            catch (Exception ex) { throw genException.GenericException.GenException(ex, _unitOfWork); }
        }

        public async Task<LitMedical?> GetByIdAsync(int id)
        {
            try { return await _unitOfWork.Repository<LitMedical>().GetByIdAsync(id); }
            catch (Exception ex) { throw genException.GenericException.GenException(ex, _unitOfWork); }
        }

        public async Task<LitMedical> AddAsync(LitMedical entity)
        {
            try
            {
                var existing = await _unitOfWork.Repository<LitMedical>().GetAsync(l => l.NumeroSerie == entity.NumeroSerie);
                if (existing != null)
                    throw new Exception("Erreur : Un lit avec ce numéro de série existe déjà.");

                entity.DateAjout = DateTime.Now;
                entity.Disponible = true;
                await _unitOfWork.Repository<LitMedical>().AddAsync(entity);
                return entity;
            }
            catch (Exception ex) { throw genException.GenericException.GenException(ex, _unitOfWork); }
        }

        public async Task<LitMedical?> UpdateAsync(int id, LitMedical entity, List<string> fieldsToUpdate)
        {
            try
            {
                var existing = await _unitOfWork.Repository<LitMedical>().GetByIdAsync(id);
                if (existing == null)
                    throw new Exception($"Erreur : Lit avec l'ID {id} introuvable.");

                if (fieldsToUpdate.Contains("NumeroSerie") && entity.NumeroSerie != existing.NumeroSerie)
                {
                    var nsExists = await _unitOfWork.Repository<LitMedical>().GetAsync(l => l.NumeroSerie == entity.NumeroSerie);
                    if (nsExists != null)
                        throw new Exception("Erreur : Ce numéro de série est déjà utilisé.");
                }

                await _unitOfWork.Repository<LitMedical>().UpdateGeneral(existing, entity, fieldsToUpdate);
                return existing;
            }
            catch (Exception ex) { throw genException.GenericException.GenException(ex, _unitOfWork); }
        }

        public async Task<bool> DeleteAsync(int id)
        {
            try
            {
                var existing = await _unitOfWork.Repository<LitMedical>().GetByIdAsync(id);
                if (existing == null)
                    throw new Exception($"Erreur : Lit avec l'ID {id} introuvable.");

                await _unitOfWork.Repository<LitMedical>().DeleteAsync(existing);
                return true;
            }
            catch (Exception ex) { throw genException.GenericException.GenException(ex, _unitOfWork); }
        }
    }
}
