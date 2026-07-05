using Accounting.Application.Services;
using Accounting.Application.Commands.CompinedEntries;
using ERP.Domain.Models.Entities.Account.Entries;
using Shared;
using Shared.Responses;

namespace Accounting.Infrastructure.Handlers.CompinedEntries;

public class CompinedEntryUpdateCommandHandler(ICompinedEntryService service) : ICommandHandler<CompinedEntryUpdateCommand, ApiResponse<Entry>>
{
    public async Task<ApiResponse<Entry>> Handle(CompinedEntryUpdateCommand request, CancellationToken cancellationToken)
    {
        return await service.Update(request);
    }
}