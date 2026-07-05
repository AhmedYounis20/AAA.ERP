using Accounting.Application.Commands.PaymentEntries;
using Accounting.Application.Services;
using Accounting.Domain.Models.Entities.Entries;

namespace Accounting.Infrastructure.Handlers.PaymentEntries;

public class PaymentEntryDeleteCommandHandler(IPaymentEntryService paymentEntryService)
    : ICommandHandler<PaymentEntryDeleteCommand, ApiResponse<Entry>>
{
    public Task<ApiResponse<Entry>> Handle(PaymentEntryDeleteCommand request, CancellationToken cancellationToken)
        => paymentEntryService.Delete(request.Id);
}
