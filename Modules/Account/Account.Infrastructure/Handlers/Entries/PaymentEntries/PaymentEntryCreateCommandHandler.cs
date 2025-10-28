using Account.Domain.Commands.Entries.PaymentEntries;

namespace Account.Infrastructure.Handlers.Entries.PaymentEntries;

public class PaymentEntryCreateCommandHandler(IPaymentEntryService service) : ICommandHandler<PaymentEntryCreateCommand, ApiResponse<Entry>>
{
    public async Task<ApiResponse<Entry>> Handle(PaymentEntryCreateCommand request, CancellationToken cancellationToken)
    {
        return await service.Create(request);
    }
}
