using Domain.Account.Commands.Entries.JournalEntries;
using Domain.Account.Models.Entities.Entries;
using Domain.Account.Services.Interfaces.Entries;
using Shared;
using Shared.Responses;

namespace Domain.Account.Handlers.Entries.JournalEntries;

public class JournalEntryCreateCommandHandler(IJournalEntryService _journalEntryService) : ICommandHandler<JournalEntryCreateCommand, ApiResponse<Entry>>
{
    public async Task<ApiResponse<Entry>> Handle(JournalEntryCreateCommand request, CancellationToken cancellationToken)
    {
        return await _journalEntryService.Create(request);
    }
}