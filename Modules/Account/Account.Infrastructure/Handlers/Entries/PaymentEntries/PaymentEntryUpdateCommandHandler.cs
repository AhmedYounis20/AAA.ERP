using Account.Domain.Commands.Entries.PaymentEntries;

namespace Account.Infrastructure.Handlers.Entries.PaymentEntries;

public class PaymentEntryUpdateCommandHandler(IPaymentEntryService service) : ICommandHandler<PaymentEntryUpdateCommand, ApiResponse<Entry>>
{
    public async Task<ApiResponse<Entry>> Handle(PaymentEntryUpdateCommand request, CancellationToken cancellationToken)
    {
        return await service.Update(request);
    }
}
