using Account.Domain.Commands.Entries.JournalEntries;

namespace Account.Infrastructure.Handlers.Entries.JournalEntries;

public class JournalEntryUpdateCommandHandler(IJournalEntryService journalEntryService) : ICommandHandler<JournalEntryUpdateCommand, ApiResponse<Entry>>
{
    public async Task<ApiResponse<Entry>> Handle(JournalEntryUpdateCommand request, CancellationToken cancellationToken)
    {
        return await journalEntryService.Update(request);
    }
}
