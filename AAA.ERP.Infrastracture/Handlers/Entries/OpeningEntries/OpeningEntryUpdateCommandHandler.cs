using Domain.Account.Commands.Entries.OpeningEntries;
using Domain.Account.Models.Entities.Entries;
using ERP.Application.Services.Account.Entries;

namespace ERP.Infrastracture.Handlers.Entries.OpeningEntries;

public class OpeningEntryUpdateCommandHandler(IOpeningEntryService openingEntryService) : ICommandHandler<OpeningEntryUpdateCommand, ApiResponse<Entry>>
{
    public async Task<ApiResponse<Entry>> Handle(OpeningEntryUpdateCommand request, CancellationToken cancellationToken)
    {
        return await openingEntryService.Update(request);
    }
}