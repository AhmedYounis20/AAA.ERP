using Domain.Account.Commands.Entries.OpeningEntries;
using Domain.Account.Models.Entities.Entries;
using ERP.Application.Services.Account.Entries;

namespace ERP.Infrastracture.Handlers.Entries.OpeningEntries;

public class OpeningEntryCreateCommandHandler(IOpeningEntryService openingEntryService) : ICommandHandler<OpeningEntryCreateCommand, ApiResponse<Entry>>
{
    public async Task<ApiResponse<Entry>> Handle(OpeningEntryCreateCommand request, CancellationToken cancellationToken)
    {
        return await openingEntryService.Create(request);
    }
}