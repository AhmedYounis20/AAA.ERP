using Accounting.Application.Commands.OpeningEntries;
using Accounting.Application.Commands.Entries;
using Accounting.Application.Services;
using Accounting.Domain.Models.Entities.Entries;

namespace Accounting.Infrastructure.Handlers.OpeningEntries;

public sealed class OpeningEntryCreateCommandHandler(IEntryWorkflowService workflow)
    : ICommandHandler<OpeningEntryCreateCommand, ApiResponse<Entry>>
{
    public Task<ApiResponse<Entry>> Handle(OpeningEntryCreateCommand request, CancellationToken cancellationToken)
    {
        var command = request.Adapt<EntryCreateCommand>();
        command.Type = EntryType.Opening;
        return workflow.Create(command);
    }
}

public sealed class OpeningEntryUpdateCommandHandler(IEntryWorkflowService workflow)
    : ICommandHandler<OpeningEntryUpdateCommand, ApiResponse<Entry>>
{
    public Task<ApiResponse<Entry>> Handle(OpeningEntryUpdateCommand request, CancellationToken cancellationToken)
        => workflow.Update(request.Adapt<EntryUpdateCommand>());
}

public sealed class OpeningEntryDeleteCommandHandler(IEntryWorkflowService workflow)
    : ICommandHandler<OpeningEntryDeleteCommand, ApiResponse<Entry>>
{
    public Task<ApiResponse<Entry>> Handle(OpeningEntryDeleteCommand request, CancellationToken cancellationToken)
        => workflow.Delete(request.Id);
}
