using Identity.Domain.Models.Dtos;
using Shared.Domain.Responses;

namespace Identity.Application.Services
{
    public interface IAuthenticationService
    {
        Guid GetUserId();
        Task<ApiResponse<LoginResponseDTO>> Login(LoginRequestDTO loginRequestDTO);
        Task<ApiResponse> Register(RegisterRequestDTO registerRequestDTO);
        Task<ApiResponse<RefreshTokenResponse>> Refresh(TokenRequestDto tokenDto);
    }
}