using Domain.Account.Commands.Entries.JournalEntries;
using Domain.Account.Models.Entities.Entries;
using ERP.Application.Services.Account.Entries;

namespace ERP.Infrastracture.Handlers.Entries.JournalEntries;

public class JournalEntryUpdateCommandHandler(IJournalEntryService journalEntryService) : ICommandHandler<JournalEntryUpdateCommand, ApiResponse<Entry>>
{
    public async Task<ApiResponse<Entry>> Handle(JournalEntryUpdateCommand request, CancellationToken cancellationToken)
    {
        return await journalEntryService.Update(request);
    }
}