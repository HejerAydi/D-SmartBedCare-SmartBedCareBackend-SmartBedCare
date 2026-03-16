using Domain.Entities;
using Infrastructure.Infrastructure;

namespace ApplicationCore.Services
{
    public class ClientServices : IClientServices
    {
        private readonly IUnitOfWork _unitOfWork;

        public ClientServices(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
        }

        public async Task<IReadOnlyList<Client>> GetAllAsync()
        {
            try
            {
                var result = await _unitOfWork.Repository<Client>().GetAllAsyncwithfilter();
                return result.ToList();
            }
            catch (Exception ex)
            {
                throw genException.GenericException.GenException(ex, _unitOfWork);
            }
        }

        public async Task<Client?> GetByIdAsync(int id)
        {
            try
            {
                return await _unitOfWork.Repository<Client>().GetByIdAsync(id);
            }
            catch (Exception ex)
            {
                throw genException.GenericException.GenException(ex, _unitOfWork);
            }
        }

        public async Task<Client> AddAsync(Client entity)
        {
            try
            {
                // Vérifier unicité CIN
                var existing = await _unitOfWork.Repository<Client>().GetAsync(c => c.Cin == entity.Cin);
                if (existing != null)
                    throw new Exception("Erreur : Un client avec ce CIN existe déjà.");

                entity.DateCreation = DateTime.Now;
                await _unitOfWork.Repository<Client>().AddAsync(entity);
                return entity;
            }
            catch (Exception ex)
            {
                throw genException.GenericException.GenException(ex, _unitOfWork);
            }
        }

        public async Task<Client?> UpdateAsync(int id, Client entity, List<string> fieldsToUpdate)
        {
            try
            {
                var existing = await _unitOfWork.Repository<Client>().GetByIdAsync(id);
                if (existing == null)
                    throw new Exception($"Erreur : Client avec l'ID {id} introuvable.");

                // Vérifier unicité CIN si modifié
                if (fieldsToUpdate.Contains("Cin") && entity.Cin != existing.Cin)
                {
                    var cinExists = await _unitOfWork.Repository<Client>().GetAsync(c => c.Cin == entity.Cin);
                    if (cinExists != null)
                        throw new Exception("Erreur : Ce CIN est déjà utilisé.");
                }

                await _unitOfWork.Repository<Client>().UpdateGeneral(existing, entity, fieldsToUpdate);
                return existing;
            }
            catch (Exception ex)
            {
                throw genException.GenericException.GenException(ex, _unitOfWork);
            }
        }

        public async Task<bool> DeleteAsync(int id)
        {
            try
            {
                var existing = await _unitOfWork.Repository<Client>().GetByIdAsync(id);
                if (existing == null)
                    throw new Exception($"Erreur : Client avec l'ID {id} introuvable.");

                await _unitOfWork.Repository<Client>().DeleteAsync(existing);
                return true;
            }
            catch (Exception ex)
            {
                throw genException.GenericException.GenException(ex, _unitOfWork);
            }
        }
    }
}
