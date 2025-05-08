using Domain.Account.Commands.Entries.PaymentEntries;
using Domain.Account.Models.Entities.Entries;
using Domain.Account.Services.Interfaces.Entries;
using Shared;
using Shared.Responses;

namespace Domain.Account.Handlers.Entries.PaymentEntries;

public class PaymentEntryUpdateCommandHandler(IPaymentEntryService service) : ICommandHandler<PaymentEntryUpdateCommand, ApiResponse<Entry>>
{
    public async Task<ApiResponse<Entry>> Handle(PaymentEntryUpdateCommand request, CancellationToken cancellationToken)
    {
        return await service.Update(request);
    }
}