using AAA.ERP.Services.Interfaces.SubLeadgers;
using Domain.Account.Commands.GLSettings;
using Domain.Account.Commands.SubLeadgers.CashInBoxes;
using Domain.Account.InputModels.Subleadgers;
using Domain.Account.Models.Entities.GLSettings;
using Domain.Account.Models.Entities.SubLeadgers;
using Domain.Account.Services.Interfaces;
using Shared;
using Shared.Responses;

namespace Domain.Account.Handlers.GLSettings;

public class GlSettingUpdateCommandHandler(IGLSettingService service): ICommandHandler<GlSettingUpdateCommand,ApiResponse<GLSetting>>
{
    public async Task<ApiResponse<GLSetting>> Handle(GlSettingUpdateCommand request, CancellationToken cancellationToken)
    {
        return await service.Update(request);
    }
}