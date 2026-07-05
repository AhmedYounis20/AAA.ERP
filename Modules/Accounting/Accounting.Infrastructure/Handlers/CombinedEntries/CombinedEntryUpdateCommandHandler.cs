using Accounting.Application.Services;
using Accounting.Application.Commands.CombinedEntries;
using ERP.Domain.Models.Entities.Account.Entries;
using Shared;
using Shared.Responses;

namespace Accounting.Infrastructure.Handlers.CombinedEntries;

public class CombinedEntryUpdateCommandHandler(ICombinedEntryService service) : ICommandHandler<CombinedEntryUpdateCommand, ApiResponse<Entry>>
{
    public async Task<ApiResponse<Entry>> Handle(CombinedEntryUpdateCommand request, CancellationToken cancellationToken)
    {
        return await service.Update(request);
    }
}