using Account.Domain.Commands.Entries.JournalEntries;

namespace Account.Infrastructure.Handlers.Entries.JournalEntries;

public class JournalEntryCreateCommandHandler(IJournalEntryService _journalEntryService) : ICommandHandler<JournalEntryCreateCommand, ApiResponse<Entry>>
{
    public async Task<ApiResponse<Entry>> Handle(JournalEntryCreateCommand request, CancellationToken cancellationToken)
    {
        return await _journalEntryService.Create(request);
    }
}
