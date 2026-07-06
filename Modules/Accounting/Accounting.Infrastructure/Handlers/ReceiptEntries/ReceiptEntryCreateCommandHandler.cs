using Accounting.Application.Commands.ReceiptEntries;
using Accounting.Application.Commands.Entries;
using Accounting.Application.Services;
using Accounting.Domain.Models.Entities.Entries;

namespace Accounting.Infrastructure.Handlers.ReceiptEntries;

public sealed class ReceiptEntryCreateCommandHandler(IComplexEntryWorkflowService workflow)
    : ICommandHandler<ReceiptEntryCreateCommand, ApiResponse<Entry>>
{
    public Task<ApiResponse<Entry>> Handle(ReceiptEntryCreateCommand request, CancellationToken cancellationToken)
    {
        var command = request.Adapt<ComplexEntryCreateCommand>();
        command.Type = EntryType.Receipt;
        return workflow.Create(command);
    }
}

public sealed class ReceiptEntryUpdateCommandHandler(IComplexEntryWorkflowService workflow)
    : ICommandHandler<ReceiptEntryUpdateCommand, ApiResponse<Entry>>
{
    public Task<ApiResponse<Entry>> Handle(ReceiptEntryUpdateCommand request, CancellationToken cancellationToken)
        => workflow.Update(request.Adapt<ComplexEntryUpdateCommand>());
}

public sealed class ReceiptEntryDeleteCommandHandler(IEntryWorkflowService workflow)
    : ICommandHandler<ReceiptEntryDeleteCommand, ApiResponse<Entry>>
{
    public Task<ApiResponse<Entry>> Handle(ReceiptEntryDeleteCommand request, CancellationToken cancellationToken)
        => workflow.Delete(request.Id);
}
