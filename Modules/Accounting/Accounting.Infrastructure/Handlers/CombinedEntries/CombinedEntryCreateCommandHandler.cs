using Accounting.Application.Services;
using Accounting.Application.Commands.CombinedEntries;
using ERP.Domain.Models.Entities.Account.Entries;
using Shared;
using Shared.Responses;

namespace Accounting.Infrastructure.Handlers.CombinedEntries;

public class CombinedEntryCreateCommandHandler(ICombinedEntryService service) : ICommandHandler<CombinedEntryCreateCommand, ApiResponse<Entry>>
{
    public async Task<ApiResponse<Entry>> Handle(CombinedEntryCreateCommand request, CancellationToken cancellationToken)
    {
        return await service.Create(request);
    }
}