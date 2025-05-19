using Domain.Account.Commands.GLSettings;
using Domain.Account.Models.Entities.GLSettings;

namespace ERP.Infrastracture.Handlers.GLSettings;

public class GlSettingUpdateCommandHandler(IGLSettingService service): ICommandHandler<GlSettingUpdateCommand,ApiResponse<GLSetting>>
{
    public async Task<ApiResponse<GLSetting>> Handle(GlSettingUpdateCommand request, CancellationToken cancellationToken)
    {
        return await service.Update(request);
    }
}