using ERP.Application.Services.Account.Entries;
using ERP.Domain.Commands.Account.Entries.PaymentEntries;
using ERP.Domain.Models.Entities.Account.Entries;


namespace ERP.Infrastracture.Handlers.Account.Entries.PaymentEntries;

public class PaymentEntryCreateCommandHandler(IPaymentEntryService service) : ICommandHandler<PaymentEntryCreateCommand, ApiResponse<Entry>>
{
    public async Task<ApiResponse<Entry>> Handle(PaymentEntryCreateCommand request, CancellationToken cancellationToken)
    {
        return await service.Create(request);
    }
}