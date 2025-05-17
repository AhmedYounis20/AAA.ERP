using Domain.Account.Commands.Entries.CompinedEntries;
using Domain.Account.Models.Entities.Entries;
using Domain.Account.Services.Interfaces.Entries;
using Shared;
using Shared.Responses;

namespace Domain.Account.Handlers.Entries.CompinedEntries;

public class CompinedEntryCreateCommandHandler(ICompinedEntryService service) : ICommandHandler<CompinedEntryCreateCommand, ApiResponse<Entry>>
{
    public async Task<ApiResponse<Entry>> Handle(CompinedEntryCreateCommand request, CancellationToken cancellationToken)
    {
        return await service.Create(request);
    }
}