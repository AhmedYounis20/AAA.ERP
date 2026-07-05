using Accounting.Application.Commands.JournalEntries;
using Accounting.Application.Services;
using ERP.Domain.Models.Entities.Account.Entries;

namespace Accounting.Infrastructure.Handlers.JournalEntries;

public class JournalEntryUpdateCommandHandler(IJournalEntryService journalEntryService)
    : ICommandHandler<JournalEntryUpdateCommand, ApiResponse<Entry>>
{
    public Task<ApiResponse<Entry>> Handle(JournalEntryUpdateCommand request, CancellationToken cancellationToken)
        => journalEntryService.Update(request);
}
