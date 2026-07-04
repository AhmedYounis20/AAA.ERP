using Accounting.Application.Commands.GLSettings;
using Accounting.Application.Services;
using ERP.Domain.Models.Entities.Account.GLSettings;

namespace Accounting.Infrastructure.Handlers.GLSettings;

public class GlSettingUpdateCommandHandler(IGLSettingService service)
    : ICommandHandler<GlSettingUpdateCommand, ApiResponse<GLSetting>>
{
    public Task<ApiResponse<GLSetting>> Handle(GlSettingUpdateCommand request, CancellationToken cancellationToken)
        => service.Update(request);
}
