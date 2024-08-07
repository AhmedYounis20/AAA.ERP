﻿using Domain.Account.DBConfiguration.DbContext;
using Domain.Account.Models.Dtos;
using Domain.Account.Utility;
using Shared.BaseEntities.Identity;
using Shared.Responses;

namespace AAA.ERP.Controllers.Identity;

[Route("api/[controller]")]
[ApiController]
public class AuthController : ControllerBase
{
    private readonly ApplicationDbContext _db;
    private string? secretKey;

    private UserManager<ApplicationUser> _userManager;
    private RoleManager<IdentityRole> _roleManager;
    public AuthController(ApplicationDbContext db, IConfiguration configuration,
        UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager)
    {
        _db = db;
        secretKey = configuration.GetValue<string>("ApiSettings:Secret");
        _userManager = userManager;
        _roleManager = roleManager;
    }

    [HttpPost("login")]
    [ProducesResponseType(typeof(ApiResponse), (int)HttpStatusCode.OK)]
    [ProducesResponseType(typeof(ApiResponse), (int)HttpStatusCode.BadRequest)]
    [ProducesResponseType(typeof(ApiResponse), (int)HttpStatusCode.NotFound)]
    public async Task<ActionResult<LoginResponseDTO>> Login([FromBody] LoginRequestDTO loginRequestDTO)
    {
        ApplicationUser? userFromDb = await _db.ApplicationUsers.FirstOrDefaultAsync(e => e.UserName.ToLower() == loginRequestDTO.UserName.ToLower());
        var _response = new ApiResponse<LoginResponseDTO>();
        if (userFromDb == null)
        {
            _response.IsSuccess = false;
            _response.StatusCode = HttpStatusCode.NotFound;
            _response.ErrorMessages = new();
            _response.ErrorMessages?.Add("User Not Found");
            return NotFound(_response);
        }
        bool isValid = await _userManager.CheckPasswordAsync(userFromDb, loginRequestDTO.Password);
        if (!isValid)
        {
            _response.StatusCode = HttpStatusCode.BadRequest;
            _response.IsSuccess = false;
            _response.ErrorMessages = new();
            _response.ErrorMessages?.Add("Username or Password is incorrect");

            return BadRequest(_response);
        }

        JwtSecurityTokenHandler tokenHandler = new JwtSecurityTokenHandler();
        byte[] key = Encoding.ASCII.GetBytes(secretKey);
        var roles = await _userManager.GetRolesAsync(userFromDb);

        SecurityTokenDescriptor tokenDescriptor = new()
        {
            Subject = new ClaimsIdentity(new Claim[] {
                new Claim("fullName", userFromDb.Name),
                new Claim("id", userFromDb.Id.ToString()),
                new Claim(ClaimTypes.Email, userFromDb.UserName.ToString()),
                new Claim(ClaimTypes.Role, roles.FirstOrDefault().ToUpper()),
            }),
            Expires = DateTime.UtcNow.AddDays(7),
            SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
        };

        SecurityToken securityToken = tokenHandler.CreateToken(tokenDescriptor);

        LoginResponseDTO response = new LoginResponseDTO
        {
            Name = userFromDb.Name,
            UserName = userFromDb.UserName,
            Email = userFromDb.Email,
            Token = tokenHandler.WriteToken(securityToken),
        };
        if (string.IsNullOrEmpty(response.Email) || string.IsNullOrEmpty(response.Token))
        {
            _response.StatusCode = HttpStatusCode.BadRequest;
            _response.IsSuccess = false;
            _response.ErrorMessages?.Add("Username or Password is incorrect");

            return BadRequest(_response);
        }

        _response.StatusCode = HttpStatusCode.Created;
        _response.IsSuccess = true;
        _response.Result = response;

        return Ok(_response);
    }
    [HttpPost("register")]
    [ProducesResponseType(typeof(ApiResponse), (int)HttpStatusCode.OK)]
    [ProducesResponseType(typeof(ApiResponse), (int)HttpStatusCode.BadRequest)]
    [ProducesResponseType(typeof(ApiResponse), (int)HttpStatusCode.NotFound)]
    public async Task<IActionResult> Register([FromBody] RegisterRequestDTO registerRequestDTO)
    {
        ApplicationUser? userFromDb = await _db.ApplicationUsers.FirstOrDefaultAsync(e => e.UserName.ToLower() == registerRequestDTO.UserName.ToLower());
        var _response = new ApiResponse<ApplicationUser>();

        if (userFromDb != null)
        {
            _response.StatusCode = HttpStatusCode.BadRequest;
            _response.IsSuccess = false;
            _response.ErrorMessages?.Add("Username already exists");

            return BadRequest(_response);
        }

        ApplicationUser newUser = new ApplicationUser
        {
            UserName = registerRequestDTO.UserName,
            Email = registerRequestDTO.Email,
            NormalizedEmail = registerRequestDTO.Email?.ToUpper(),
            Name = registerRequestDTO.Name,
        };

        var result = await _userManager.CreateAsync(newUser, registerRequestDTO.Password ?? "");
        if (result.Succeeded)
        {
            if (!_roleManager.RoleExistsAsync(SD.Role_Admin).GetAwaiter().GetResult())
            {
                await _roleManager.CreateAsync(new IdentityRole { Name = SD.Role_Admin, NormalizedName = SD.Role_Admin.ToUpper() });
                await _roleManager.CreateAsync(new IdentityRole { Name = SD.Role_Customer, NormalizedName = SD.Role_Customer.ToUpper() });
            }

            if (registerRequestDTO.Role?.ToUpper() == SD.Role_Admin)
                await _userManager.AddToRoleAsync(newUser, SD.Role_Admin);
            else
                await _userManager.AddToRoleAsync(newUser, SD.Role_Customer);

            _response.IsSuccess = true;
            _response.StatusCode = HttpStatusCode.Created;
            return Ok(_response);
        }

        _response.StatusCode = HttpStatusCode.BadRequest;
        _response.IsSuccess = false;
        _response.ErrorMessages?.Add("Error while registeration");

        return BadRequest();
    }



}