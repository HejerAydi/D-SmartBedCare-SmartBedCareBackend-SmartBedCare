using Domain.Entities;
using Infrastructure.Infrastructure;

namespace ApplicationCore.Services
{
    public class UtilisateurServices : IUtilisateurServices
    {
        private readonly IUnitOfWork _unitOfWork;

        public UtilisateurServices(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
        }

        public async Task<IReadOnlyList<Utilisateur>> GetAllAsync()
        {
            try
            {
                var result = await _unitOfWork.Repository<Utilisateur>().GetAllAsyncwithfilter();
                return result.ToList();
            }
            catch (Exception ex)
            {
                throw genException.GenericException.GenException(ex, _unitOfWork);
            }
        }

        public async Task<Utilisateur?> GetByIdAsync(int id)
        {
            try
            {
                return await _unitOfWork.Repository<Utilisateur>().GetByIdAsync(id);
            }
            catch (Exception ex)
            {
                throw genException.GenericException.GenException(ex, _unitOfWork);
            }
        }

        public async Task<Utilisateur?> GetByEmailAsync(string email)
        {
            try
            {
                return await _unitOfWork.Repository<Utilisateur>().GetAsync(u => u.Email == email);
            }
            catch (Exception ex)
            {
                throw genException.GenericException.GenException(ex, _unitOfWork);
            }
        }

        public async Task<Utilisateur> AddAsync(Utilisateur entity)
        {
            try
            {
                // Vérifier si l'email existe déjà
                var existing = await _unitOfWork.Repository<Utilisateur>().GetAsync(u => u.Email == entity.Email);
                if (existing != null)
                    throw new Exception("Erreur : Un utilisateur avec cet email existe déjà.");

                // Hasher le mot de passe
                entity.Password = BCrypt.Net.BCrypt.HashPassword(entity.Password);
                entity.DateCreation = DateTime.Now;

                await _unitOfWork.Repository<Utilisateur>().AddAsync(entity);
                return entity;
            }
            catch (Exception ex)
            {
                throw genException.GenericException.GenException(ex, _unitOfWork);
            }
        }

        public async Task<Utilisateur?> UpdateAsync(int id, Utilisateur entity, List<string> fieldsToUpdate)
        {
            try
            {
                var existing = await _unitOfWork.Repository<Utilisateur>().GetByIdAsync(id);
                if (existing == null)
                    throw new Exception($"Erreur : Utilisateur avec l'ID {id} introuvable.");

                // Vérifier unicité email si modifié
                if (fieldsToUpdate.Contains("Email") && entity.Email != existing.Email)
                {
                    var emailExists = await _unitOfWork.Repository<Utilisateur>().GetAsync(u => u.Email == entity.Email);
                    if (emailExists != null)
                        throw new Exception("Erreur : Cet email est déjà utilisé.");
                }

                await _unitOfWork.Repository<Utilisateur>().UpdateGeneral(existing, entity, fieldsToUpdate);
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
                var existing = await _unitOfWork.Repository<Utilisateur>().GetByIdAsync(id);
                if (existing == null)
                    throw new Exception($"Erreur : Utilisateur avec l'ID {id} introuvable.");

                await _unitOfWork.Repository<Utilisateur>().DeleteAsync(existing);
                return true;
            }
            catch (Exception ex)
            {
                throw genException.GenericException.GenException(ex, _unitOfWork);
            }
        }

        public async Task<bool> ToggleActiveAsync(int id)
        {
            try
            {
                var existing = await _unitOfWork.Repository<Utilisateur>().GetByIdAsync(id);
                if (existing == null)
                    throw new Exception($"Erreur : Utilisateur avec l'ID {id} introuvable.");

                existing.IsActive = !existing.IsActive;
                await _unitOfWork.Repository<Utilisateur>().UpdateAsync(existing);
                return existing.IsActive;
            }
            catch (Exception ex)
            {
                throw genException.GenericException.GenException(ex, _unitOfWork);
            }
        }
    }
}
