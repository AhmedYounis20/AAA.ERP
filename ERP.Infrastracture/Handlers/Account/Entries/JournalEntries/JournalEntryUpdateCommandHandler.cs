using ERP.Application.Services.Account.Entries;
using ERP.Domain.Commands.Account.Entries.JournalEntries;
using ERP.Domain.Models.Entities.Account.Entries;

namespace ERP.Infrastracture.Handlers.Account.Entries.JournalEntries;

public class JournalEntryUpdateCommandHandler(IJournalEntryService journalEntryService) : ICommandHandler<JournalEntryUpdateCommand, ApiResponse<Entry>>
{
    public async Task<ApiResponse<Entry>> Handle(JournalEntryUpdateCommand request, CancellationToken cancellationToken)
    {
        return await journalEntryService.Update(request);
    }
}