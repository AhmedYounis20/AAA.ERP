using ERP.Application.Services.Account.Entries;
using ERP.Domain.Commands.Account.Entries.PaymentEntries;
using ERP.Domain.Models.Entities.Account.Entries;

namespace ERP.Infrastracture.Handlers.Account.Entries.PaymentEntries;

public class PaymentEntryUpdateCommandHandler(IPaymentEntryService service) : ICommandHandler<PaymentEntryUpdateCommand, ApiResponse<Entry>>
{
    public async Task<ApiResponse<Entry>> Handle(PaymentEntryUpdateCommand request, CancellationToken cancellationToken)
    {
        return await service.Update(request);
    }
}