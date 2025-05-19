using Domain.Account.Commands.Entries.PaymentEntries;
using Domain.Account.Models.Entities.Entries;
using ERP.Application.Services.Account.Entries;


namespace ERP.Infrastracture.Handlers.Entries.PaymentEntries;

public class PaymentEntryCreateCommandHandler(IPaymentEntryService service) : ICommandHandler<PaymentEntryCreateCommand, ApiResponse<Entry>>
{
    public async Task<ApiResponse<Entry>> Handle(PaymentEntryCreateCommand request, CancellationToken cancellationToken)
    {
        return await service.Create(request);
    }
}