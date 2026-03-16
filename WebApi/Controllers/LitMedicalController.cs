using ApplicationCore.Services;
using AutoMapper;
using Domain.DTOs;
using Domain.Entities;
using Infrastructure.Infrastructure;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class LitMedicalController : ControllerBase
    {
        private readonly ILitMedicalServices _litServices;
        private readonly IUnitOfWork _uof;
        private readonly IMapper _mapper;

        public LitMedicalController(ILitMedicalServices litServices, IUnitOfWork uof, IMapper mapper)
        {
            _litServices = litServices; _uof = uof; _mapper = mapper;
        }

        [HttpGet("GetAll")]
        public async Task<IActionResult> GetAll([FromQuery] bool? disponible = null)
        {
            try
            {
                var lits = await _litServices.GetAllAsync(disponible);
                var dtos = _mapper.Map<IReadOnlyList<LitMedicalDTO>>(lits);
                return Ok(new ApiResponse<IReadOnlyList<LitMedicalDTO>>("Récupération réussie", dtos));
            }
            catch (Exception ex) { return BadRequest(ex.Message); }
        }

        [HttpGet("GetById/{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            try
            {
                var lit = await _litServices.GetByIdAsync(id);
                if (lit == null) return NotFound($"Lit ID {id} introuvable.");
                return Ok(new ApiResponse<LitMedicalDTO>("Récupération réussie", _mapper.Map<LitMedicalDTO>(lit)));
            }
            catch (Exception ex) { return BadRequest(ex.Message); }
        }

        [HttpPost("Add")]
        public async Task<IActionResult> Add([FromBody] CreateLitMedicalDTO dto)
        {
            try
            {
                _uof.BeginTransaction();
                var entity = _mapper.Map<LitMedical>(dto);
                var created = await _litServices.AddAsync(entity);
                _uof.CommitTransaction();
                return Ok(new ApiResponse<LitMedicalDTO>("Lit créé avec succès", _mapper.Map<LitMedicalDTO>(created)));
            }
            catch (Exception ex) { _uof.RollbackTransaction(); return BadRequest(ex.Message); }
        }

        [HttpPut("Update/{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] UpdateLitMedicalDTO dto)
        {
            var allowedFields = new List<string> { "NumeroSerie", "NombreFonction", "TypeLit", "TypeMatelas", "PrixLocation", "FraisTransport", "ImageMatelas", "Disponible" };
            try
            {
                _uof.BeginTransaction();
                var entity = _mapper.Map<LitMedical>(dto);
                var updated = await _litServices.UpdateAsync(id, entity, allowedFields);
                if (updated == null) { _uof.RollbackTransaction(); return NotFound($"Lit ID {id} introuvable."); }
                _uof.CommitTransaction();
                return Ok(new ApiResponse<LitMedicalDTO>("Lit mis à jour avec succès", _mapper.Map<LitMedicalDTO>(updated)));
            }
            catch (Exception ex) { _uof.RollbackTransaction(); return BadRequest(ex.Message); }
        }

        [HttpDelete("Delete/{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                _uof.BeginTransaction();
                await _litServices.DeleteAsync(id);
                _uof.CommitTransaction();
                return Ok(new ApiResponse<bool>("Lit supprimé avec succès", true));
            }
            catch (Exception ex) { _uof.RollbackTransaction(); return BadRequest(ex.Message); }
        }
    }
}
