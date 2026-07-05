using Accounting.Application.Commands.PaymentEntries;
using Accounting.Application.Services;
using Accounting.Domain.Models.Entities.Entries;

namespace Accounting.Infrastructure.Handlers.PaymentEntries;

public class PaymentEntryCreateCommandHandler(IPaymentEntryService service)
    : ICommandHandler<PaymentEntryCreateCommand, ApiResponse<Entry>>
{
    public Task<ApiResponse<Entry>> Handle(PaymentEntryCreateCommand request, CancellationToken cancellationToken)
        => service.Create(request);
}
