using ERP.Application.Services.Account.Entries;
using ERP.Domain.Commands.Account.Entries.OpeningEntries;
using ERP.Domain.Models.Entities.Account.Entries;

namespace ERP.Infrastracture.Handlers.Account.Entries.OpeningEntries;

public class OpeningEntryUpdateCommandHandler(IOpeningEntryService openingEntryService) : ICommandHandler<OpeningEntryUpdateCommand, ApiResponse<Entry>>
{
    public async Task<ApiResponse<Entry>> Handle(OpeningEntryUpdateCommand request, CancellationToken cancellationToken)
    {
        return await openingEntryService.Update(request);
    }
}