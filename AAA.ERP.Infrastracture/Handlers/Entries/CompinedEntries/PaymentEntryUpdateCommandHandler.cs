using Domain.Account.Commands.Entries.CompinedEntries;
using Domain.Account.Models.Entities.Entries;
using ERP.Application.Services.Account.Entries;
using Shared;
using Shared.Responses;

namespace ERP.Infrastracture.Handlers.Entries.CompinedEntries;

public class CompinedEntryUpdateCommandHandler(ICompinedEntryService service) : ICommandHandler<CompinedEntryUpdateCommand, ApiResponse<Entry>>
{
    public async Task<ApiResponse<Entry>> Handle(CompinedEntryUpdateCommand request, CancellationToken cancellationToken)
    {
        return await service.Update(request);
    }
}