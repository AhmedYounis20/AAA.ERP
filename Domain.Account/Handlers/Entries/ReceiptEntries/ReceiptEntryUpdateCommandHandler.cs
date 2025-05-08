using Domain.Account.Commands.Entries.ReceiptEntries;
using Domain.Account.Models.Entities.Entries;
using Domain.Account.Services.Interfaces.Entries;
using Shared;
using Shared.Responses;

namespace Domain.Account.Handlers.Entries.ReceiptEntries;

public class ReceiptEntryUpdateCommandHandler(IReceiptEntryService service) : ICommandHandler<ReceiptEntryUpdateCommand, ApiResponse<Entry>>
{
    public async Task<ApiResponse<Entry>> Handle(ReceiptEntryUpdateCommand request, CancellationToken cancellationToken)
    {
        return await service.Update(request);
    }
}