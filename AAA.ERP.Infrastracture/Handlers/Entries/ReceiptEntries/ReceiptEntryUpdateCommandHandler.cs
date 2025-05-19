using Domain.Account.Commands.Entries.ReceiptEntries;
using Domain.Account.Models.Entities.Entries;
using ERP.Application.Services.Account.Entries;

namespace ERP.Infrastracture.Handlers.Entries.ReceiptEntries;

public class ReceiptEntryUpdateCommandHandler(IReceiptEntryService service) : ICommandHandler<ReceiptEntryUpdateCommand, ApiResponse<Entry>>
{
    public async Task<ApiResponse<Entry>> Handle(ReceiptEntryUpdateCommand request, CancellationToken cancellationToken)
    {
        return await service.Update(request);
    }
}