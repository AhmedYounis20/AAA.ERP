using Accounting.Application.Commands.JournalEntries;
using Accounting.Application.Services;
using Accounting.Domain.Models.Entities.Entries;

namespace Accounting.Infrastructure.Handlers.JournalEntries;

public class JournalEntryDeleteCommandHandler(IJournalEntryService journalEntryService)
    : ICommandHandler<JournalEntryDeleteCommand, ApiResponse<Entry>>
{
    public Task<ApiResponse<Entry>> Handle(JournalEntryDeleteCommand request, CancellationToken cancellationToken)
        => journalEntryService.Delete(request.Id);
}
