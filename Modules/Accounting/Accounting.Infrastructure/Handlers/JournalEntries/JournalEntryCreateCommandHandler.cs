using Accounting.Application.Commands.JournalEntries;
using Accounting.Application.Commands.Entries;
using Accounting.Application.Services;
using Accounting.Domain.Models.Entities.Entries;

namespace Accounting.Infrastructure.Handlers.JournalEntries;

public sealed class JournalEntryCreateCommandHandler(IEntryWorkflowService workflow)
    : ICommandHandler<JournalEntryCreateCommand, ApiResponse<Entry>>
{
    public Task<ApiResponse<Entry>> Handle(JournalEntryCreateCommand request, CancellationToken cancellationToken)
    {
        var command = request.Adapt<EntryCreateCommand>();
        command.Type = EntryType.Journal;
        return workflow.Create(command);
    }
}

public sealed class JournalEntryUpdateCommandHandler(IEntryWorkflowService workflow)
    : ICommandHandler<JournalEntryUpdateCommand, ApiResponse<Entry>>
{
    public Task<ApiResponse<Entry>> Handle(JournalEntryUpdateCommand request, CancellationToken cancellationToken)
        => workflow.Update(request.Adapt<EntryUpdateCommand>());
}

public sealed class JournalEntryDeleteCommandHandler(IEntryWorkflowService workflow)
    : ICommandHandler<JournalEntryDeleteCommand, ApiResponse<Entry>>
{
    public Task<ApiResponse<Entry>> Handle(JournalEntryDeleteCommand request, CancellationToken cancellationToken)
        => workflow.Delete(request.Id);
}
