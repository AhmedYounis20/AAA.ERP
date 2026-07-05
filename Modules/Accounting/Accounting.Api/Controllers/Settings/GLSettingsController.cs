using Accounting.Application.Commands;
using Accounting.Application.Commands.GLSettings;
using Accounting.Application.Services;
using Accounting.Application.Validators.GLSettings;
using Accounting.Domain.Models.Entities.GLSettings;
using ERP.BuildingBlocks.Api.Extensions;
using ERP.BuildingBlocks.Application.Constants;
using ERP.BuildingBlocks.Application.Responses;
using Mapster;

namespace Accounting.Api.Controllers.Settings;

/// <summary>Read/update API for general ledger settings (hybrid CQRS).</summary>
[Route("api/[controller]")]
[ApiController]
[Authorize]
public class GLSettingsController(
    ISender sender,
    IGLSettingReadService reads,
    GLSettingInputValidator validator) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> Get() => this.ToResult(await reads.Get());

    [HttpPut]
    public async Task<IActionResult> Update(GlSettingUpdateCommand input)
    {
        var validationResult = validator.Validate(input.Adapt<GLSettingInputModel>());
        if (!validationResult.IsValid)
        {
            return BadRequest(new ApiResponse<GLSetting>
            {
                IsSuccess = false,
                StatusCode = HttpStatusCode.BadRequest,
                Errors = validationResult.Errors.Select(e => new MessageTemplate { MessageKey = e.ErrorMessage }).ToList()
            });
        }

        return this.ToResult(await sender.Send(input), successMessageKey: SuccessMessages.UpdatedSuccessfully);
    }
}
