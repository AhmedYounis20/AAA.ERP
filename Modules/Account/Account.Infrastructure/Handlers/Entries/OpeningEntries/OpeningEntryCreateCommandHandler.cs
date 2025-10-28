using Account.Domain.Commands.Entries.OpeningEntries;

namespace Account.Infrastructure.Handlers.Entries.OpeningEntries;

public class OpeningEntryCreateCommandHandler(IOpeningEntryService openingEntryService) : ICommandHandler<OpeningEntryCreateCommand, ApiResponse<Entry>>
{
    public async Task<ApiResponse<Entry>> Handle(OpeningEntryCreateCommand request, CancellationToken cancellationToken)
    {
        return await openingEntryService.Create(request);
    }
}
