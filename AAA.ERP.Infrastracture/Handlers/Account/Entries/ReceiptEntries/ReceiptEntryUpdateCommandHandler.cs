using ERP.Application.Services.Account.Entries;
using ERP.Domain.Commands.Account.Entries.ReceiptEntries;
using ERP.Domain.Models.Entities.Account.Entries;

namespace ERP.Infrastracture.Handlers.Account.Entries.ReceiptEntries;

public class ReceiptEntryUpdateCommandHandler(IReceiptEntryService service) : ICommandHandler<ReceiptEntryUpdateCommand, ApiResponse<Entry>>
{
    public async Task<ApiResponse<Entry>> Handle(ReceiptEntryUpdateCommand request, CancellationToken cancellationToken)
    {
        return await service.Update(request);
    }
}