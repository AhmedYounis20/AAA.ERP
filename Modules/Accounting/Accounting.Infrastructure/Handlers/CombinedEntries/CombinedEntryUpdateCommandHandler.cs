using Accounting.Application.Services;
using Accounting.Application.Commands.CombinedEntries;
using Accounting.Domain.Models.Entities.Entries;
using Shared;
using ERP.BuildingBlocks.Application.Responses;

namespace Accounting.Infrastructure.Handlers.CombinedEntries;

public class CombinedEntryUpdateCommandHandler(ICombinedEntryService service) : ICommandHandler<CombinedEntryUpdateCommand, ApiResponse<Entry>>
{
    public async Task<ApiResponse<Entry>> Handle(CombinedEntryUpdateCommand request, CancellationToken cancellationToken)
    {
        return await service.Update(request);
    }
}