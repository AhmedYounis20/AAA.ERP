using Domain.Account.Models.Dtos;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace Domain.Account.Services.Identity;

public class AuthenticationService : IAuthenticationService
{
    private readonly ICurrentUser _currentUser;
    private readonly IConfiguration _configuration;
    private readonly ApplicationDbContext _db;
    private readonly string? _secretKey;
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly RoleManager<IdentityRole> _roleManager;

    public AuthenticationService(
        ICurrentUser currentUser,
        ApplicationDbContext db,
        IConfiguration configuration,
        UserManager<ApplicationUser> userManager,
        RoleManager<IdentityRole> roleManager)
    {
        _currentUser = currentUser;
        _configuration = configuration;
        _userManager = userManager;
        _db = db;
        _secretKey = configuration.GetValue<string>("ApiSettings:Secret");
        _roleManager = roleManager;
    }

    public Guid GetUserId() => _currentUser.UserId ?? Guid.Empty;

    public string GenerateAccessToken(IEnumerable<Claim> claims)
    {
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_secretKey));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(
            issuer: _configuration["Jwt:Issuer"],
            audience: _configuration["Jwt:Audience"],
            claims: claims,
            expires: DateTime.Now.AddMinutes(15),
            signingCredentials: creds
        );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }

    public string GenerateRefreshToken()
    {
        var randomNumber = new byte[32];
        using var rng = RandomNumberGenerator.Create();
        rng.GetBytes(randomNumber);
        return Convert.ToBase64String(randomNumber);
    }

    public async Task<ApiResponse<LoginResponseDTO>> Login(LoginRequestDTO loginRequestDTO)
    {
        var response = new ApiResponse<LoginResponseDTO>();

        try
        {
            var userFromDb = await _db.ApplicationUsers
                .FirstOrDefaultAsync(e => !string.IsNullOrEmpty(e.UserName) && e.UserName.ToLower() == loginRequestDTO.UserName.ToLower());

            if (userFromDb == null)
            {
                response.IsSuccess = false;
                response.StatusCode = HttpStatusCode.NotFound;
                response.Errors = new List<MessageTemplate> { new MessageTemplate { MessageKey = "USER_NOT_FOUND" } };
                return response;
            }

            bool isValid = await _userManager.CheckPasswordAsync(userFromDb, loginRequestDTO.Password);
            if (!isValid)
            {
                response.IsSuccess = false;
                response.StatusCode = HttpStatusCode.BadRequest;
                response.Errors = new List<MessageTemplate> { new MessageTemplate { MessageKey = "INCORRENCT_EMAIL_OR_PASSWORD" } };
                return response;
            }

            var roles = await _userManager.GetRolesAsync(userFromDb);
            var claims = roles.Select(role => new Claim(ClaimTypes.Role, role)).ToList();
            claims.Add(new Claim(ClaimTypes.NameIdentifier, userFromDb.Id.ToString()));
            claims.Add(new Claim(ClaimTypes.Name, userFromDb.UserName));

            var accessToken = GenerateAccessToken(claims);
            var refreshToken = GenerateRefreshToken();

            userFromDb.RefreshToken = refreshToken;
            userFromDb.RefreshTokenExpiryTime = DateTime.UtcNow.AddDays(7);
            _db.ApplicationUsers.Update(userFromDb);
            await _db.SaveChangesAsync();

            var loginResponse = new LoginResponseDTO
            {
                Email = userFromDb.Email,
                AccessToken = accessToken,
                RefreshToken = refreshToken
            };

            response.IsSuccess = true;
            response.StatusCode = HttpStatusCode.OK;
            response.Result = loginResponse;
        }
        catch (Exception ex)
        {
            response.IsSuccess = false;
            response.StatusCode = HttpStatusCode.InternalServerError;
            response.Errors = new List<MessageTemplate> { new MessageTemplate { MessageKey = "Login failed." }, new MessageTemplate { MessageKey = ex.Message } };
        }

        return response;
    }

    public async Task<ApiResponse> Register(RegisterRequestDTO registerRequestDTO)
    {
        var response = new ApiResponse
        {
            Errors = new List<MessageTemplate>()
        };

        var userFromDb = await _db.ApplicationUsers
            .FirstOrDefaultAsync(e => !string.IsNullOrEmpty(e.Email) && e.Email.ToLower() == registerRequestDTO.Email.ToLower());

        if (userFromDb != null)
        {
            response.StatusCode = HttpStatusCode.BadRequest;
            response.IsSuccess = false;
            response.Errors.Add(new MessageTemplate { MessageKey = "UsernameAlreadyExists" });
            return response;
        }

        using var transaction = await _db.Database.BeginTransactionAsync();

        try
        {
            var newUser = new ApplicationUser
            {
                UserName = registerRequestDTO.Email,
                Email = registerRequestDTO.Email,
                NormalizedEmail = registerRequestDTO.Email.ToUpper(),
                RefreshToken = string.Empty
            };

            var result = await _userManager.CreateAsync(newUser, registerRequestDTO.Password ?? "");

            if (!result.Succeeded)
            {
                response.StatusCode = HttpStatusCode.BadRequest;
                response.IsSuccess = false;
                response.Errors.Add(new MessageTemplate { MessageKey = "UserCreationFailed" });
                return response;
            }

            if (!await _roleManager.RoleExistsAsync(SD.Role_Admin))
                await _roleManager.CreateAsync(new IdentityRole(SD.Role_Admin));

            await _userManager.AddToRoleAsync(newUser, SD.Role_Admin);

            await transaction.CommitAsync();

            response.IsSuccess = true;
            response.StatusCode = HttpStatusCode.Created;
            return response;
        }
        catch (Exception ex)
        {
            await transaction.RollbackAsync();

            response.IsSuccess = false;
            response.StatusCode = HttpStatusCode.InternalServerError;
            response.Errors.Add(new MessageTemplate { MessageKey = $"Transaction failed: {ex.Message}" });
            return response;
        }
    }

    public async Task<ApiResponse<RefreshTokenResponse>> Refresh(TokenRequestDto tokenDto)
    {
        var response = new ApiResponse<RefreshTokenResponse>
        {
            IsSuccess = true,
            StatusCode = HttpStatusCode.OK,
        };

        var principal = GetPrincipalFromExpiredToken(tokenDto.AccessToken);
        if (principal == null)
        {
            response.IsSuccess = false;
            response.StatusCode = HttpStatusCode.BadRequest;
            response.Errors = new List<MessageTemplate> { new MessageTemplate { MessageKey = "INVALID_ACCESS_TOKEN_OR_REFRESH_TOKEN" } };
            return response;
        }

        var username = principal.Identity?.Name;
        var user = await _userManager.FindByNameAsync(username);

        if (user == null || user.RefreshToken != tokenDto.RefreshToken || user.RefreshTokenExpiryTime <= DateTime.UtcNow)
        {
            response.IsSuccess = false;
            response.StatusCode = HttpStatusCode.BadRequest;
            response.Errors = new List<MessageTemplate> { new MessageTemplate { MessageKey = "INVALID_REFRESH_TOKEN" } };
            return response;
        }

        var newAccessToken = GenerateAccessToken(principal.Claims);
        var newRefreshToken = GenerateRefreshToken();

        user.RefreshToken = newRefreshToken;
        user.RefreshTokenExpiryTime = DateTime.UtcNow.AddDays(7);
        await _userManager.UpdateAsync(user);

        response.Result = new RefreshTokenResponse
        {
            AccessToken = newAccessToken,
            RefreshToken = newRefreshToken,
        };

        return response;
    }

    private ClaimsPrincipal? GetPrincipalFromExpiredToken(string token)
    {
        var tokenValidationParameters = new TokenValidationParameters
        {
            ValidateAudience = false,
            ValidateIssuer = false,
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_secretKey)),
            ValidateLifetime = false
        };

        var tokenHandler = new JwtSecurityTokenHandler();
        var principal = tokenHandler.ValidateToken(token, tokenValidationParameters, out SecurityToken securityToken);
        return securityToken is JwtSecurityToken ? principal : null;
    }
}
