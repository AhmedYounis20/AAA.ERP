using AAA.ERP.InputModels;
using AAA.ERP.Models.Entities.Currencies;
using AAA.ERP.Models.Entities.GLSettings;
using AAA.ERP.Resources;
using AAA.ERP.Responses;
using AAA.ERP.Services.BaseServices.interfaces;
using AAA.ERP.Services.Interfaces;
using AAA.ERP.Validators.InputValidators;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Localization;

namespace AAA.ERP.Controllers;

[Route("api/[controller]")]
[ApiController]
[Authorize]
public class GLSettingsController : ControllerBase
{
    GLSettingValidator _validator;
    IMapper _mapper;
    IStringLocalizer<Resource> _localizer;
    IGLSettingService _service;
    public GLSettingsController(IGLSettingService service, GLSettingValidator validator, IStringLocalizer<Resource> localizer, IMapper mapper)
    {
        _validator = validator;
        _mapper = mapper;
        _localizer = localizer;
        _service = service;

    }

    [HttpGet]
    public virtual async Task<IActionResult> Get()
    {
        var result = await _service.Get();
        return StatusCode((int)result.StatusCode, result);
    }

    [HttpPut]
    public virtual async Task<IActionResult> Update(GLSettingInputModel input)
    {
        var validationResult = _validator.Validate(input);
        if (validationResult.IsValid)
        {
            var entity = _mapper.Map<GLSetting>(input);
            var userId = User.Claims.FirstOrDefault(e => e.Type == "id")?.Value;
            entity.ModifiedBy = Guid.Parse(userId ?? "");
            var result = await _service.Update(entity);
            return StatusCode((int)result.StatusCode, result);
        }
        else
        {
            return BadRequest(
               new ApiResponse
               {
                   IsSuccess = false,
                   StatusCode = HttpStatusCode.BadRequest,
                   ErrorMessages = validationResult.Errors.Select(e => _localizer[e.ErrorMessage].Value).ToList(),
               });
        }
    }
}