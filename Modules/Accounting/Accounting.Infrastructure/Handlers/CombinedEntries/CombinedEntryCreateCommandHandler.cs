using Accounting.Application.Commands.CombinedEntries;
using Accounting.Application.Commands.Entries;
using Accounting.Application.Services;
using Accounting.Domain.Models.Entities.Entries;

namespace Accounting.Infrastructure.Handlers.CombinedEntries;

public sealed class CombinedEntryCreateCommandHandler(IComplexEntryWorkflowService workflow)
    : ICommandHandler<CombinedEntryCreateCommand, ApiResponse<Entry>>
{
    public Task<ApiResponse<Entry>> Handle(CombinedEntryCreateCommand request, CancellationToken cancellationToken)
    {
        var command = request.Adapt<ComplexEntryCreateCommand>();
        command.Type = EntryType.Combined;
        return workflow.Create(command);
    }
}

public sealed class CombinedEntryUpdateCommandHandler(IComplexEntryWorkflowService workflow)
    : ICommandHandler<CombinedEntryUpdateCommand, ApiResponse<Entry>>
{
    public Task<ApiResponse<Entry>> Handle(CombinedEntryUpdateCommand request, CancellationToken cancellationToken)
        => workflow.Update(request.Adapt<ComplexEntryUpdateCommand>());
}

public sealed class CombinedEntryDeleteCommandHandler(IEntryWorkflowService workflow)
    : ICommandHandler<CombinedEntryDeleteCommand, ApiResponse<Entry>>
{
    public Task<ApiResponse<Entry>> Handle(CombinedEntryDeleteCommand request, CancellationToken cancellationToken)
        => workflow.Delete(request.Id);
}
