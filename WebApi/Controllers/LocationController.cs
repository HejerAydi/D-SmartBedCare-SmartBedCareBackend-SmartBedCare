using ApplicationCore.Services;
using AutoMapper;
using Domain.DTOs;
using Domain.Entities;
using Infrastructure.Infrastructure;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class LocationController : ControllerBase
    {
        private readonly ILocationServices _locationServices;
        private readonly IUnitOfWork _uof;
        private readonly IMapper _mapper;

        public LocationController(ILocationServices locationServices, IUnitOfWork uof, IMapper mapper)
        {
            _locationServices = locationServices; _uof = uof; _mapper = mapper;
        }

        [HttpGet("GetAll")]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                var locations = await _locationServices.GetAllAsync();
                var dtos = _mapper.Map<IReadOnlyList<LocationDTO>>(locations);
                return Ok(new ApiResponse<IReadOnlyList<LocationDTO>>("Récupération réussie", dtos));
            }
            catch (Exception ex) { return BadRequest(ex.Message); }
        }

        [HttpGet("GetById/{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            try
            {
                var location = await _locationServices.GetByIdAsync(id);
                if (location == null) return NotFound($"Location ID {id} introuvable.");
                return Ok(new ApiResponse<LocationDTO>("Récupération réussie", _mapper.Map<LocationDTO>(location)));
            }
            catch (Exception ex) { return BadRequest(ex.Message); }
        }

        [HttpPost("Add")]
        public async Task<IActionResult> Add([FromBody] CreateLocationDTO dto)
        {
            try
            {
                _uof.BeginTransaction();
                var userId = int.Parse(User.FindFirst("sub")?.Value ?? "0");
                var created = await _locationServices.AddAsync(dto, userId);
                _uof.CommitTransaction();
                return Ok(new ApiResponse<LocationDTO>("Location créée avec succès", _mapper.Map<LocationDTO>(created)));
            }
            catch (Exception ex) { _uof.RollbackTransaction(); return BadRequest(ex.Message); }
        }

        [HttpPut("Update/{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] UpdateLocationDTO dto)
        {
            var allowedFields = new List<string> { "DateLivraison", "DateRecuperation", "Statut" };
            try
            {
                _uof.BeginTransaction();
                var entity = _mapper.Map<Location>(dto);
                var updated = await _locationServices.UpdateAsync(id, entity, allowedFields);
                if (updated == null) { _uof.RollbackTransaction(); return NotFound($"Location ID {id} introuvable."); }
                _uof.CommitTransaction();
                return Ok(new ApiResponse<LocationDTO>("Location mise à jour avec succès", _mapper.Map<LocationDTO>(updated)));
            }
            catch (Exception ex) { _uof.RollbackTransaction(); return BadRequest(ex.Message); }
        }

        [HttpPatch("Recuperer/{id}")]
        public async Task<IActionResult> Recuperer(int id)
        {
            try
            {
                _uof.BeginTransaction();
                await _locationServices.RecupererAsync(id);
                _uof.CommitTransaction();
                return Ok(new ApiResponse<bool>("Récupération effectuée avec succès", true));
            }
            catch (Exception ex) { _uof.RollbackTransaction(); return BadRequest(ex.Message); }
        }
    }
}
