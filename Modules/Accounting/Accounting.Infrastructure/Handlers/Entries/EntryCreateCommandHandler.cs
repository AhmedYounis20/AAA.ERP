using Accounting.Application.Commands.Entries;
using Accounting.Application.Services;
using Accounting.Domain.Models.Entities.Entries;

namespace Accounting.Infrastructure.Handlers.Entries;

public sealed class EntryCreateCommandHandler(IComplexEntryWorkflowService workflow)
    : ICommandHandler<ComplexEntryCreateCommand, ApiResponse<Entry>>
{
    public Task<ApiResponse<Entry>> Handle(ComplexEntryCreateCommand request, CancellationToken cancellationToken)
        => workflow.Create(request);
}

public sealed class EntryUpdateCommandHandler(IComplexEntryWorkflowService workflow)
    : ICommandHandler<ComplexEntryUpdateCommand, ApiResponse<Entry>>
{
    public Task<ApiResponse<Entry>> Handle(ComplexEntryUpdateCommand request, CancellationToken cancellationToken)
        => workflow.Update(request);
}

public sealed class ComplexEntryDeleteCommandHandler(IEntryWorkflowService workflow)
    : ICommandHandler<ComplexEntryDeleteCommand, ApiResponse<Entry>>
{
    public Task<ApiResponse<Entry>> Handle(ComplexEntryDeleteCommand request, CancellationToken cancellationToken)
        => workflow.Delete(request.Id);
}
