using ERP.Application.Services.Account.Entries;
using ERP.Domain.Commands.Account.Entries.CompinedEntries;
using ERP.Domain.Models.Entities.Account.Entries;
using Shared;
using Shared.Responses;

namespace ERP.Infrastracture.Handlers.Account.Entries.CompinedEntries;

public class CompinedEntryCreateCommandHandler(ICompinedEntryService service) : ICommandHandler<CompinedEntryCreateCommand, ApiResponse<Entry>>
{
    public async Task<ApiResponse<Entry>> Handle(CompinedEntryCreateCommand request, CancellationToken cancellationToken)
    {
        return await service.Create(request);
    }
}