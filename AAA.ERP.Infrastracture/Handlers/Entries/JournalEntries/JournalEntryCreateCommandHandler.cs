using Domain.Account.Commands.Entries.JournalEntries;
using Domain.Account.Models.Entities.Entries;
using ERP.Application.Services.Account.Entries;

namespace ERP.Infrastracture.Handlers.Entries.JournalEntries;

public class JournalEntryCreateCommandHandler(IJournalEntryService _journalEntryService) : ICommandHandler<JournalEntryCreateCommand, ApiResponse<Entry>>
{
    public async Task<ApiResponse<Entry>> Handle(JournalEntryCreateCommand request, CancellationToken cancellationToken)
    {
        return await _journalEntryService.Create(request);
    }
}