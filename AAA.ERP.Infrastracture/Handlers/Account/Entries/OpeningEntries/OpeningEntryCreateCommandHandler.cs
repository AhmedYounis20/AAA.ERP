using ERP.Application.Services.Account.Entries;
using ERP.Domain.Commands.Account.Entries.OpeningEntries;
using ERP.Domain.Models.Entities.Account.Entries;

namespace ERP.Infrastracture.Handlers.Account.Entries.OpeningEntries;

public class OpeningEntryCreateCommandHandler(IOpeningEntryService openingEntryService) : ICommandHandler<OpeningEntryCreateCommand, ApiResponse<Entry>>
{
    public async Task<ApiResponse<Entry>> Handle(OpeningEntryCreateCommand request, CancellationToken cancellationToken)
    {
        return await openingEntryService.Create(request);
    }
}