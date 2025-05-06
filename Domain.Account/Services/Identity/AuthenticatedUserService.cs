using Domain.Account.DBConfiguration.DbContext;
using Domain.Account.Models.Dtos;
using Domain.Account.Utility;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Shared.BaseEntities.Identity;
using Shared.Responses;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace Domain.Account.Services.Identity
{
    public class AuthenticationService : IAuthenticationService
    {
        private readonly IHttpContextAccessor _contextAccessor;
        private readonly IConfiguration _configuration;
        private readonly ApplicationDbContext _db;
        private string? secretKey;

        private UserManager<ApplicationUser> _userManager;
        private RoleManager<IdentityRole> _roleManager;

        public AuthenticationService(IHttpContextAccessor contextAccessor, ApplicationDbContext db, IConfiguration configuration,
        UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            _contextAccessor = contextAccessor;
            _configuration = configuration;
            _userManager = userManager;
            _db = db;
            secretKey = configuration.GetValue<string>("ApiSettings:Secret");
            _roleManager = roleManager;
        }

        public Guid GetUserId()
        {
            var userId = _contextAccessor.HttpContext?.User?.Claims?.FirstOrDefault(e => e.Type == ClaimTypes.NameIdentifier)?.Value;
            return Guid.Parse(!string.IsNullOrEmpty(userId) ? userId : $"{Guid.Empty}");
        }

        public string GenerateAccessToken(IEnumerable<Claim> claims)
        {
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: _configuration["Jwt:Issuer"],
                audience: _configuration["Jwt:Audience"],
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(15),
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
            var _response = new ApiResponse<LoginResponseDTO>();

            try
            {
                var userFromDb = await _db.ApplicationUsers
                    .FirstOrDefaultAsync(e => !string.IsNullOrEmpty(e.UserName) && e.UserName.ToLower() == loginRequestDTO.UserName.ToLower());

                if (userFromDb == null)
                {
                    _response.IsSuccess = false;
                    _response.StatusCode = HttpStatusCode.NotFound;
                    _response.ErrorMessages = new() { "USER_NOT_FOUND" };
                    return _response;
                }

                bool isValid = await _userManager.CheckPasswordAsync(userFromDb, loginRequestDTO.Password);
                if (!isValid)
                {
                    _response.IsSuccess = false;
                    _response.StatusCode = HttpStatusCode.BadRequest;
                    _response.ErrorMessages = new() { "INCORRENCT_EMAIL_OR_PASSWORD" };
                    return _response;
                }

                // Get roles and claims
                var roles = await _userManager.GetRolesAsync(userFromDb);
                var claims = roles.Select(role => new Claim(ClaimTypes.Role, role)).ToList();
                claims.Add(new Claim(ClaimTypes.NameIdentifier, userFromDb.Id.ToString()));
                claims.Add(new Claim(ClaimTypes.Name, userFromDb.UserName));

                // Generate tokens
                var accessToken = GenerateAccessToken(claims);
                var refreshToken = GenerateRefreshToken();

                // Store refresh token in DB
                userFromDb.RefreshToken = refreshToken;
                userFromDb.RefreshTokenExpiryTime = DateTime.UtcNow.AddDays(7); // valid for 7 days
                _db.ApplicationUsers.Update(userFromDb);
                await _db.SaveChangesAsync();

                // Prepare response
                var response = new LoginResponseDTO
                {
                    Email = userFromDb.Email,
                    AccessToken = accessToken,
                    RefreshToken = refreshToken
                };

                _response.IsSuccess = true;
                _response.StatusCode = HttpStatusCode.OK;
                _response.Result = response;
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.StatusCode = HttpStatusCode.InternalServerError;
                _response.ErrorMessages = new() { "Login failed.", ex.Message };
            }

            return _response;
        }

        public async Task<ApiResponse> Register(RegisterRequestDTO registerRequestDTO)
        {
            var _response = new ApiResponse
            {
                ErrorMessages = new List<string>()
            };

            // Check if user exists
            var userFromDb = await _db.ApplicationUsers
                .FirstOrDefaultAsync(e => !string.IsNullOrEmpty(e.Email) && e.Email.ToLower() == registerRequestDTO.Email.ToLower());

            if (userFromDb != null)
            {
                _response.StatusCode = HttpStatusCode.BadRequest;
                _response.IsSuccess = false;
                _response.ErrorMessages.Add("Username already exists");
                return _response;
            }

            // Start DB transaction
            using var transaction = await _db.Database.BeginTransactionAsync();

            try
            {
                // 1. Create ApplicationUser
                var newUser = new ApplicationUser
                {
                    UserName = registerRequestDTO.Email,
                    Email = registerRequestDTO.Email,
                    NormalizedEmail = registerRequestDTO.Email.ToUpper(),
                };

                var result = await _userManager.CreateAsync(newUser, registerRequestDTO.Password ?? "");

                if (!result.Succeeded)
                {
                    _response.StatusCode = HttpStatusCode.BadRequest;
                    _response.IsSuccess = false;
                    _response.ErrorMessages.Add("User creation failed");
                    return _response;
                }

                // 3. Ensure Roles
                if (!await _roleManager.RoleExistsAsync(SD.Role_Admin))
                    await _roleManager.CreateAsync(new IdentityRole(SD.Role_Admin));

                // 4. Assign Roles
                await _userManager.AddToRoleAsync(newUser, SD.Role_Admin);

                // 5. Commit transaction
                await transaction.CommitAsync();

                _response.IsSuccess = true;
                _response.StatusCode = HttpStatusCode.Created;
                return _response;
            }
            catch (Exception ex)
            {
                // Rollback if any error
                await transaction.RollbackAsync();

                _response.IsSuccess = false;
                _response.StatusCode = HttpStatusCode.InternalServerError;
                _response.ErrorMessages.Add($"Transaction failed: {ex.Message}");
                return _response;
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
                response.ErrorMessages = ["INVALID_ACCESS_TOKEN_OR_REFRESH_TOKEN"];
                return response;
            }

            var username = principal.Identity.Name;
            var user = await _userManager.FindByNameAsync(username);

            if (user == null || user.RefreshToken != tokenDto.RefreshToken || user.RefreshTokenExpiryTime <= DateTime.UtcNow)
            {
                response.IsSuccess = false;
                response.StatusCode = HttpStatusCode.BadRequest;
                response.ErrorMessages = ["INVALID_REFRESH_TOKEN"];
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

        private ClaimsPrincipal GetPrincipalFromExpiredToken(string token)
        {
            var tokenValidationParameters = new TokenValidationParameters
            {
                ValidateAudience = false,
                ValidateIssuer = false,
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey)),
                ValidateLifetime = false // We're validating an expired token
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            var principal = tokenHandler.ValidateToken(token, tokenValidationParameters, out SecurityToken securityToken);
            return securityToken is JwtSecurityToken jwtToken ? principal : null;
        }
    }
}
