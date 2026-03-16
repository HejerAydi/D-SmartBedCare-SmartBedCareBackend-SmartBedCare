using ApplicationCore.Services;
using AutoMapper;
using Domain.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class HistoriqueController : ControllerBase
    {
        private readonly IHistoriqueServices _historiqueServices;
        private readonly IMapper _mapper;

        public HistoriqueController(IHistoriqueServices historiqueServices, IMapper mapper)
        {
            _historiqueServices = historiqueServices; _mapper = mapper;
        }

        [HttpGet("GetAll")]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                var historiques = await _historiqueServices.GetAllAsync();
                return Ok(new ApiResponse<IReadOnlyList<HistoriqueDTO>>("Récupération réussie", _mapper.Map<IReadOnlyList<HistoriqueDTO>>(historiques)));
            }
            catch (Exception ex) { return BadRequest(ex.Message); }
        }

        [HttpGet("GetByTable/{tableName}")]
        public async Task<IActionResult> GetByTable(string tableName)
        {
            try
            {
                var historiques = await _historiqueServices.GetByTableAsync(tableName);
                return Ok(new ApiResponse<IReadOnlyList<HistoriqueDTO>>("Récupération réussie", _mapper.Map<IReadOnlyList<HistoriqueDTO>>(historiques)));
            }
            catch (Exception ex) { return BadRequest(ex.Message); }
        }
    }
}
