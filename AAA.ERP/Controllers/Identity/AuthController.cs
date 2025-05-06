using Domain.Account.DBConfiguration.DbContext;
using Domain.Account.Models.Dtos;
using Domain.Account.Services.Identity;
using Domain.Account.Utility;
using Shared.BaseEntities.Identity;
using Shared.Responses;

namespace AAA.ERP.Controllers.Identity;

[Route("api/[controller]")]
[ApiController]
public class AuthController : ControllerBase
{
    private readonly IAuthenticationService _authenticationService;

    public AuthController(IAuthenticationService authenticationService)
    {
        _authenticationService = authenticationService;
    }

    [HttpPost("login")]
    [ProducesResponseType(typeof(ApiResponse), (int)HttpStatusCode.OK)]
    [ProducesResponseType(typeof(ApiResponse), (int)HttpStatusCode.BadRequest)]
    [ProducesResponseType(typeof(ApiResponse), (int)HttpStatusCode.NotFound)]
    public async Task<ActionResult<LoginResponseDTO>> Login([FromBody] LoginRequestDTO loginRequestDTO)
    {
        var result = await _authenticationService.Login(loginRequestDTO);
        return StatusCode((int)result.StatusCode, result);
    }

    [HttpPost("register")]
    [ProducesResponseType(typeof(ApiResponse), (int)HttpStatusCode.OK)]
    [ProducesResponseType(typeof(ApiResponse), (int)HttpStatusCode.BadRequest)]
    [ProducesResponseType(typeof(ApiResponse), (int)HttpStatusCode.NotFound)]
    public async Task<IActionResult> Register([FromBody] RegisterRequestDTO registerRequestDTO)
    {
        var result = await _authenticationService.Register(registerRequestDTO);
        return StatusCode((int)result.StatusCode, result);
    }

    [HttpPost("refresh")]
    public async Task<IActionResult> Refresh(TokenRequestDto tokenDto)
    {
        var result = await _authenticationService.Refresh(tokenDto);
        return StatusCode((int)result.StatusCode, result);
    }
}