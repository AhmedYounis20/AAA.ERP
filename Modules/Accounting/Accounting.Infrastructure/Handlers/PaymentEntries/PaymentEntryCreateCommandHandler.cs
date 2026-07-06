using Accounting.Application.Commands.PaymentEntries;
using Accounting.Application.Commands.Entries;
using Accounting.Application.Services;
using Accounting.Domain.Models.Entities.Entries;

namespace Accounting.Infrastructure.Handlers.PaymentEntries;

public sealed class PaymentEntryCreateCommandHandler(IComplexEntryWorkflowService workflow)
    : ICommandHandler<PaymentEntryCreateCommand, ApiResponse<Entry>>
{
    public Task<ApiResponse<Entry>> Handle(PaymentEntryCreateCommand request, CancellationToken cancellationToken)
    {
        var command = request.Adapt<ComplexEntryCreateCommand>();
        command.Type = EntryType.Payment;
        return workflow.Create(command);
    }
}

public sealed class PaymentEntryUpdateCommandHandler(IComplexEntryWorkflowService workflow)
    : ICommandHandler<PaymentEntryUpdateCommand, ApiResponse<Entry>>
{
    public Task<ApiResponse<Entry>> Handle(PaymentEntryUpdateCommand request, CancellationToken cancellationToken)
        => workflow.Update(request.Adapt<ComplexEntryUpdateCommand>());
}

public sealed class PaymentEntryDeleteCommandHandler(IEntryWorkflowService workflow)
    : ICommandHandler<PaymentEntryDeleteCommand, ApiResponse<Entry>>
{
    public Task<ApiResponse<Entry>> Handle(PaymentEntryDeleteCommand request, CancellationToken cancellationToken)
        => workflow.Delete(request.Id);
}
