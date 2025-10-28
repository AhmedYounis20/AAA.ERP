using Account.Domain.Commands.GLSettings;
using Account.Domain.Models.Entities.GLSettings;

namespace Account.Infrastructure.Handlers.GLSettings;

public class GlSettingUpdateCommandHandler(IGLSettingService service) : ICommandHandler<GlSettingUpdateCommand, ApiResponse<GLSetting>>
{
    public async Task<ApiResponse<GLSetting>> Handle(GlSettingUpdateCommand request, CancellationToken cancellationToken)
    {
        return await service.Update(request);
    }
}
