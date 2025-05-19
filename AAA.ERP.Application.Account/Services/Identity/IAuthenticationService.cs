using Domain.Account.Models.Dtos;
using Shared.Responses;

namespace ERP.Application.Services.Identity
{
    public interface IAuthenticationService
    {
        Guid GetUserId();
        Task<ApiResponse<LoginResponseDTO>> Login(LoginRequestDTO loginRequestDTO);
        Task<ApiResponse> Register(RegisterRequestDTO registerRequestDTO);
        Task<ApiResponse<RefreshTokenResponse>> Refresh(TokenRequestDto tokenDto);
    }
}