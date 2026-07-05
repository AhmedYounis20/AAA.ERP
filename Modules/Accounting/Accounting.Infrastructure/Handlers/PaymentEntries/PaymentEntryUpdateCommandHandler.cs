using Accounting.Application.Commands.PaymentEntries;
using Accounting.Application.Services;
using ERP.Domain.Models.Entities.Account.Entries;

namespace Accounting.Infrastructure.Handlers.PaymentEntries;

public class PaymentEntryUpdateCommandHandler(IPaymentEntryService service)
    : ICommandHandler<PaymentEntryUpdateCommand, ApiResponse<Entry>>
{
    public Task<ApiResponse<Entry>> Handle(PaymentEntryUpdateCommand request, CancellationToken cancellationToken)
        => service.Update(request);
}
