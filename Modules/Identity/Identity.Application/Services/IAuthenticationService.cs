using Identity.Application.Models.Dtos;
using Shared.Responses;

namespace Identity.Application;

/// <summary>
/// Authentication service contract for login, registration, and token refresh.
/// </summary>
public interface IAuthenticationService
{
    /// <summary>Gets the current authenticated user identifier.</summary>
    Guid GetUserId();

    /// <summary>Authenticates a user and returns access and refresh tokens.</summary>
    Task<ApiResponse<LoginResponseDTO>> Login(LoginRequestDTO loginRequestDTO);

    /// <summary>Registers a new application user.</summary>
    Task<ApiResponse> Register(RegisterRequestDTO registerRequestDTO);

    /// <summary>Refreshes an expired access token using a valid refresh token.</summary>
    Task<ApiResponse<RefreshTokenResponse>> Refresh(TokenRequestDto tokenDto);
}
