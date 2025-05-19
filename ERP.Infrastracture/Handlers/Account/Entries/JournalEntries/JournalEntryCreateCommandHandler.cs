using ERP.Application.Services.Account.Entries;
using ERP.Domain.Commands.Account.Entries.JournalEntries;
using ERP.Domain.Models.Entities.Account.Entries;

namespace ERP.Infrastracture.Handlers.Account.Entries.JournalEntries;

public class JournalEntryCreateCommandHandler(IJournalEntryService _journalEntryService) : ICommandHandler<JournalEntryCreateCommand, ApiResponse<Entry>>
{
    public async Task<ApiResponse<Entry>> Handle(JournalEntryCreateCommand request, CancellationToken cancellationToken)
    {
        return await _journalEntryService.Create(request);
    }
}