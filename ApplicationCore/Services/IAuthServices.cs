using Domain.DTOs;

namespace ApplicationCore.Services
{
    public interface IAuthServices
    {
        Task<LoginResponseDTO> LoginAsync(LoginRequestDTO request);
    }
}
