using Domain.DTOs;
using Domain.Entities;
using Infrastructure.Infrastructure;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace ApplicationCore.Services
{
    public class AuthServices : IAuthServices
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IConfiguration _configuration;

        public AuthServices(IUnitOfWork unitOfWork, IConfiguration configuration)
        {
            _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
            _configuration = configuration;
        }

        public async Task<LoginResponseDTO> LoginAsync(LoginRequestDTO request)
        {
            try
            {
                // Récupérer l'utilisateur par email
                var utilisateur = await _unitOfWork.Repository<Utilisateur>()
                    .GetAsync(u => u.Email == request.Email);

                if (utilisateur == null)
                    throw new Exception("Email ou mot de passe incorrect.");

                if (!utilisateur.IsActive)
                    throw new Exception("Ce compte est désactivé. Contactez l'administrateur.");

                // Vérifier le mot de passe
                if (!BCrypt.Net.BCrypt.Verify(request.Password, utilisateur.Password))
                    throw new Exception("Email ou mot de passe incorrect.");

                // Générer le token JWT
                var (token, expiration) = GenerateJwtToken(utilisateur);

                return new LoginResponseDTO
                {
                    Id = utilisateur.Id,
                    Nom = utilisateur.Nom,
                    Prenom = utilisateur.Prenom,
                    Email = utilisateur.Email,
                    Role = utilisateur.Role,
                    Token = token,
                    Expiration = expiration
                };
            }
            catch (Exception ex)
            {
                throw genException.GenericException.GenException(ex, _unitOfWork);
            }
        }

        private (string token, DateTime expiration) GenerateJwtToken(Utilisateur utilisateur)
        {
            var jwtSettings = _configuration.GetSection("JwtSettings");
            var secretKey = jwtSettings["SecretKey"]!;
            var issuer = jwtSettings["Issuer"];
            var audience = jwtSettings["Audience"];
            var expirationMinutes = int.Parse(jwtSettings["ExpirationMinutes"] ?? "60");

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey));
            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var expiration = DateTime.UtcNow.AddMinutes(expirationMinutes);

            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, utilisateur.Id.ToString()),
                new Claim(JwtRegisteredClaimNames.Email, utilisateur.Email),
                new Claim(ClaimTypes.Role, utilisateur.Role),
                new Claim("nom", utilisateur.Nom),
                new Claim("prenom", utilisateur.Prenom),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };

            var token = new JwtSecurityToken(
                issuer: issuer,
                audience: audience,
                claims: claims,
                expires: expiration,
                signingCredentials: credentials
            );

            return (new JwtSecurityTokenHandler().WriteToken(token), expiration);
        }
    }
}
