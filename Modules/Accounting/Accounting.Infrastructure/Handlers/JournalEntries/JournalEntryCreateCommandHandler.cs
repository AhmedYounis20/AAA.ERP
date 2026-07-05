using Accounting.Application.Commands.JournalEntries;
using Accounting.Application.Services;
using ERP.Domain.Models.Entities.Account.Entries;

namespace Accounting.Infrastructure.Handlers.JournalEntries;

public class JournalEntryCreateCommandHandler(IJournalEntryService journalEntryService)
    : ICommandHandler<JournalEntryCreateCommand, ApiResponse<Entry>>
{
    public Task<ApiResponse<Entry>> Handle(JournalEntryCreateCommand request, CancellationToken cancellationToken)
        => journalEntryService.Create(request);
}
