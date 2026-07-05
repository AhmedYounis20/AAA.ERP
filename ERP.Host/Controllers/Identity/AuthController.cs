using Identity.Application.Models.Dtos;
using Identity.Application;
using Shared.Responses;

namespace ERP.Host.Controllers.Identity;

/// <summary>
/// Authentication endpoints for login, registration, and token refresh.
/// </summary>
[Route("api/[controller]")]
[ApiController]
public class AuthController : ControllerBase
{
    private readonly IAuthenticationService _authenticationService;

    public AuthController(IAuthenticationService authenticationService)
    {
        _authenticationService = authenticationService;
    }

    /// <summary>Authenticates a user and returns JWT access and refresh tokens.</summary>
    [HttpPost("login")]
    [ProducesResponseType(typeof(ApiResponse), (int)HttpStatusCode.OK)]
    [ProducesResponseType(typeof(ApiResponse), (int)HttpStatusCode.BadRequest)]
    [ProducesResponseType(typeof(ApiResponse), (int)HttpStatusCode.NotFound)]
    public async Task<ActionResult<LoginResponseDTO>> Login([FromBody] LoginRequestDTO loginRequestDTO)
    {
        var result = await _authenticationService.Login(loginRequestDTO);
        return StatusCode((int)result.StatusCode, result);
    }

    /// <summary>Registers a new user account.</summary>
    [HttpPost("register")]
    [ProducesResponseType(typeof(ApiResponse), (int)HttpStatusCode.OK)]
    [ProducesResponseType(typeof(ApiResponse), (int)HttpStatusCode.BadRequest)]
    [ProducesResponseType(typeof(ApiResponse), (int)HttpStatusCode.NotFound)]
    public async Task<IActionResult> Register([FromBody] RegisterRequestDTO registerRequestDTO)
    {
        var result = await _authenticationService.Register(registerRequestDTO);
        return StatusCode((int)result.StatusCode, result);
    }

    /// <summary>Exchanges a valid refresh token for a new access token pair.</summary>
    [HttpPost("refresh")]
    public async Task<IActionResult> Refresh(TokenRequestDto tokenDto)
    {
        var result = await _authenticationService.Refresh(tokenDto);
        return StatusCode((int)result.StatusCode, result);
    }
}
