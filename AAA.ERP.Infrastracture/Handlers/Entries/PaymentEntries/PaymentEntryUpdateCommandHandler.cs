using Domain.Account.Commands.Entries.PaymentEntries;
using Domain.Account.Models.Entities.Entries;
using ERP.Application.Services.Account.Entries;

namespace ERP.Infrastracture.Handlers.Entries.PaymentEntries;

public class PaymentEntryUpdateCommandHandler(IPaymentEntryService service) : ICommandHandler<PaymentEntryUpdateCommand, ApiResponse<Entry>>
{
    public async Task<ApiResponse<Entry>> Handle(PaymentEntryUpdateCommand request, CancellationToken cancellationToken)
    {
        return await service.Update(request);
    }
}