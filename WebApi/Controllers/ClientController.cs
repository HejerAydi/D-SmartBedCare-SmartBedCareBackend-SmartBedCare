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
    public class ClientController : ControllerBase
    {
        private readonly IClientServices _clientServices;
        private readonly IUnitOfWork _uof;
        private readonly IMapper _mapper;

        public ClientController(IClientServices clientServices, IUnitOfWork uof, IMapper mapper)
        {
            _clientServices = clientServices;
            _uof = uof;
            _mapper = mapper;
        }

        [HttpGet("GetAll")]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                var clients = await _clientServices.GetAllAsync();
                var dtos = _mapper.Map<IReadOnlyList<ClientDTO>>(clients);
                return Ok(new ApiResponse<IReadOnlyList<ClientDTO>>("Récupération réussie", dtos));
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("GetById/{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            try
            {
                var client = await _clientServices.GetByIdAsync(id);
                if (client == null)
                    return NotFound($"Aucun client trouvé avec l'identifiant {id}.");

                var dto = _mapper.Map<ClientDTO>(client);
                return Ok(new ApiResponse<ClientDTO>("Récupération réussie", dto));
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("Add")]
        public async Task<IActionResult> Add([FromBody] CreateClientDTO dto)
        {
            try
            {
                _uof.BeginTransaction();
                var entity = _mapper.Map<Client>(dto);
                var created = await _clientServices.AddAsync(entity);
                _uof.CommitTransaction();

                var resultDto = _mapper.Map<ClientDTO>(created);
                return Ok(new ApiResponse<ClientDTO>("Client créé avec succès", resultDto));
            }
            catch (Exception ex)
            {
                _uof.RollbackTransaction();
                return BadRequest(ex.Message);
            }
        }

        [HttpPut("Update/{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] UpdateClientDTO dto)
        {
            var allowedFields = new List<string> { "NomComplet", "Cin", "Tel1", "Tel2", "Adresse", "Latitude", "Longitude", "PieceJointe" };

            try
            {
                _uof.BeginTransaction();
                var entity = _mapper.Map<Client>(dto);
                var updated = await _clientServices.UpdateAsync(id, entity, allowedFields);
                if (updated == null)
                {
                    _uof.RollbackTransaction();
                    return NotFound($"Aucun client trouvé avec l'identifiant {id}.");
                }
                _uof.CommitTransaction();

                var resultDto = _mapper.Map<ClientDTO>(updated);
                return Ok(new ApiResponse<ClientDTO>("Client mis à jour avec succès", resultDto));
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
                var result = await _clientServices.DeleteAsync(id);
                _uof.CommitTransaction();
                return Ok(new ApiResponse<bool>("Client supprimé avec succès", result));
            }
            catch (Exception ex)
            {
                _uof.RollbackTransaction();
                return BadRequest(ex.Message);
            }
        }
    }
}
