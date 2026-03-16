using ApplicationCore.Services;
using Domain.DTOs;
using Infrastructure.Infrastructure;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthServices _authServices;
        private readonly IUnitOfWork _uof;

        public AuthController(IAuthServices authServices, IUnitOfWork uof)
        {
            _authServices = authServices;
            _uof = uof;
        }

        [HttpPost("Login")]
        [AllowAnonymous]
        public async Task<IActionResult> Login([FromBody] LoginRequestDTO request)
        {
            try
            {
                var response = await _authServices.LoginAsync(request);
                return Ok(new ApiResponse<LoginResponseDTO>("Connexion réussie", response));
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("Logout")]
        [Authorize]
        public IActionResult Logout()
        {
            // JWT est stateless — le logout se gère côté client en supprimant le token
            return Ok(new ApiResponse<string>("Déconnexion réussie", "Veuillez supprimer le token côté client."));
        }

        [HttpGet("Me")]
        [Authorize]
        public IActionResult Me()
        {
            var id = User.FindFirst("sub")?.Value;
            var email = User.FindFirst(System.Security.Claims.ClaimTypes.Email)?.Value
                     ?? User.FindFirst("email")?.Value;
            var role = User.FindFirst(System.Security.Claims.ClaimTypes.Role)?.Value;
            var nom = User.FindFirst("nom")?.Value;
            var prenom = User.FindFirst("prenom")?.Value;

            return Ok(new ApiResponse<object>("Utilisateur connecté", new { id, email, role, nom, prenom }));
        }
    }
}
