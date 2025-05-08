using Domain.Account.Commands.Entries.ReceiptEntries;
using Domain.Account.Models.Entities.Entries;
using Domain.Account.Services.Interfaces.Entries;
using Shared;
using Shared.Responses;

namespace Domain.Account.Handlers.Entries.ReceiptEntries;

public class ReceiptEntryCreateCommandHandler(IReceiptEntryService service) : ICommandHandler<ReceiptEntryCreateCommand, ApiResponse<Entry>>
{
    public async Task<ApiResponse<Entry>> Handle(ReceiptEntryCreateCommand request, CancellationToken cancellationToken)
    {
        return await service.Create(request);
    }
}