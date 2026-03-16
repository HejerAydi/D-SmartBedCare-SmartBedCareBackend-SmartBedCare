using ApplicationCore.Services;
using AutoMapper;
using Domain.DTOs;
using Domain.Entities;
using Infrastructure.Infrastructure;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UtilisateurController : ControllerBase
    {
        private readonly IUtilisateurServices _utilisateurServices;
        private readonly IUnitOfWork _uof;
        private readonly IMapper _mapper;

        public UtilisateurController(IUtilisateurServices utilisateurServices, IUnitOfWork uof, IMapper mapper)
        {
            _utilisateurServices = utilisateurServices;
            _uof = uof ?? throw new ArgumentNullException(nameof(uof));
            _mapper = mapper;
        }

        [HttpGet("GetAll")]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                var utilisateurs = await _utilisateurServices.GetAllAsync();
                var dtos = _mapper.Map<IReadOnlyList<UtilisateurDTO>>(utilisateurs);
                return Ok(new ApiResponse<IReadOnlyList<UtilisateurDTO>>("Récupération réussie", dtos));
            }
            catch (Exception ex)
            {
                _uof.RollbackTransaction();
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("GetById/{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            try
            {
                var utilisateur = await _utilisateurServices.GetByIdAsync(id);
                if (utilisateur == null)
                    return NotFound($"Aucun utilisateur trouvé avec l'identifiant {id}.");

                var dto = _mapper.Map<UtilisateurDTO>(utilisateur);
                return Ok(new ApiResponse<UtilisateurDTO>("Récupération réussie", dto));
            }
            catch (Exception ex)
            {
                _uof.RollbackTransaction();
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("Add")]
        public async Task<IActionResult> Add([FromBody] CreateUtilisateurDTO dto)
        {
            try
            {
                _uof.BeginTransaction();
                var entity = _mapper.Map<Utilisateur>(dto);
                var created = await _utilisateurServices.AddAsync(entity);
                _uof.CommitTransaction();

                var resultDto = _mapper.Map<UtilisateurDTO>(created);
                return Ok(new ApiResponse<UtilisateurDTO>("Utilisateur créé avec succès", resultDto));
            }
            catch (Exception ex)
            {
                _uof.RollbackTransaction();
                return BadRequest(ex.Message);
            }
        }

        [HttpPut("Update/{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] UpdateUtilisateurDTO dto)
        {
            var allowedFields = new List<string> { "Nom", "Prenom", "Email", "Role", "IsActive" };

            try
            {
                _uof.BeginTransaction();
                var entity = _mapper.Map<Utilisateur>(dto);
                var updated = await _utilisateurServices.UpdateAsync(id, entity, allowedFields);
                if (updated == null)
                {
                    _uof.RollbackTransaction();
                    return NotFound($"Aucun utilisateur trouvé avec l'identifiant {id}.");
                }
                _uof.CommitTransaction();

                var resultDto = _mapper.Map<UtilisateurDTO>(updated);
                return Ok(new ApiResponse<UtilisateurDTO>("Utilisateur mis à jour avec succès", resultDto));
            }
            catch (Exception ex)
            {
                _uof.RollbackTransaction();
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete("Delete/{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                _uof.BeginTransaction();
                var result = await _utilisateurServices.DeleteAsync(id);
                _uof.CommitTransaction();
                return Ok(new ApiResponse<bool>("Utilisateur supprimé avec succès", result));
            }
            catch (Exception ex)
            {
                _uof.RollbackTransaction();
                return BadRequest(ex.Message);
            }
        }

        [HttpPatch("ToggleActive/{id}")]
        public async Task<IActionResult> ToggleActive(int id)
        {
            try
            {
                _uof.BeginTransaction();
                var isActive = await _utilisateurServices.ToggleActiveAsync(id);
                _uof.CommitTransaction();
                return Ok(new ApiResponse<bool>($"Utilisateur {(isActive ? "activé" : "désactivé")} avec succès", isActive));
            }
            catch (Exception ex)
            {
                _uof.RollbackTransaction();
                return BadRequest(ex.Message);
            }
        }
    }
}
