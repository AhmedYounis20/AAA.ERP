using Domain.Account.Commands.Entries.JournalEntries;
using Domain.Account.Models.Entities.Entries;
using Domain.Account.Services.Interfaces.Entries;
using Shared;
using Shared.Responses;

namespace Domain.Account.Handlers.Entries.JournalEntries;

public class JournalEntryUpdateCommandHandler(IJournalEntryService journalEntryService) : ICommandHandler<JournalEntryUpdateCommand, ApiResponse<Entry>>
{
    public async Task<ApiResponse<Entry>> Handle(JournalEntryUpdateCommand request, CancellationToken cancellationToken)
    {
        return await journalEntryService.Update(request);
    }
}