using ERP.Domain.Commands.Account.GLSettings;
using ERP.Domain.Models.Entities.Account.GLSettings;

namespace ERP.Infrastracture.Handlers.Account.GLSettings;

public class GlSettingUpdateCommandHandler(IGLSettingService service) : ICommandHandler<GlSettingUpdateCommand, ApiResponse<GLSetting>>
{
    public async Task<ApiResponse<GLSetting>> Handle(GlSettingUpdateCommand request, CancellationToken cancellationToken)
    {
        return await service.Update(request);
    }
}