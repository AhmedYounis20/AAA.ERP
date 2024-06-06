using AutoMapper;
using Domain.Account.Commands.GLSettings;
using Domain.Account.InputModels;
using Domain.Account.Models.Entities.GLSettings;
using Domain.Account.Services.Interfaces;
using Domain.Account.Validators.InputValidators;
using Mapster;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Localization;
using Shared.Resources;
using Shared.Responses;

namespace AAA.ERP.Controllers;

[Route("api/[controller]")]
[ApiController]
[Authorize]
public class GLSettingsController : ControllerBase
{
    GLSettingInputValidator _validator;
    IMapper _mapper;
    IStringLocalizer<Resource> _localizer;
    IGLSettingService _service;
    public GLSettingsController(IGLSettingService service,
        GLSettingInputValidator validator,
        IStringLocalizer<Resource> localizer,
        IMapper mapper)
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
    public virtual async Task<IActionResult> Update(GlSettingUpdateCommand input)
    {
        var toValidate = input.Adapt<GLSettingInputModel>();
        var validationResult = _validator.Validate(toValidate);
        if (validationResult.IsValid)
        {
            var entity = _mapper.Map<GLSetting>(input);
            var userId = User.Claims.FirstOrDefault(e => e.Type == "id")?.Value;
            entity.ModifiedBy = Guid.Parse(userId ?? "");
            var result = await _service.Update(input);
            return StatusCode((int)result.StatusCode, result);
        }
        else
        {
            return BadRequest(
               new ApiResponse <GLSetting>
               {
                   IsSuccess = false,
                   StatusCode = HttpStatusCode.BadRequest,
                   ErrorMessages = validationResult.Errors.Select(e => _localizer[e.ErrorMessage].Value).ToList(),
               });
        }
    }
}