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
    public class PaiementController : ControllerBase
    {
        private readonly IPaiementServices _paiementServices;
        private readonly IUnitOfWork _uof;
        private readonly IMapper _mapper;

        public PaiementController(IPaiementServices paiementServices, IUnitOfWork uof, IMapper mapper)
        {
            _paiementServices = paiementServices; _uof = uof; _mapper = mapper;
        }

        [HttpGet("GetByLocation/{locationId}")]
        public async Task<IActionResult> GetByLocation(int locationId)
        {
            try
            {
                var paiements = await _paiementServices.GetByLocationAsync(locationId);
                var dtos = _mapper.Map<IReadOnlyList<PaiementDTO>>(paiements);
                return Ok(new ApiResponse<IReadOnlyList<PaiementDTO>>("Récupération réussie", dtos));
            }
            catch (Exception ex) { return BadRequest(ex.Message); }
        }

        [HttpPost("Add")]
        public async Task<IActionResult> Add([FromBody] CreatePaiementDTO dto)
        {
            try
            {
                _uof.BeginTransaction();
                var entity = _mapper.Map<Paiement>(dto);
                var created = await _paiementServices.AddAsync(entity);
                _uof.CommitTransaction();
                return Ok(new ApiResponse<PaiementDTO>("Paiement ajouté avec succès", _mapper.Map<PaiementDTO>(created)));
            }
            catch (Exception ex) { _uof.RollbackTransaction(); return BadRequest(ex.Message); }
        }
    }
}
