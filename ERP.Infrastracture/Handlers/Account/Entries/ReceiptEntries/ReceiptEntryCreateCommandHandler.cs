using ERP.Application.Services.Account.Entries;
using ERP.Domain.Commands.Account.Entries.ReceiptEntries;
using ERP.Domain.Models.Entities.Account.Entries;
using Shared;
using Shared.Responses;

namespace ERP.Infrastracture.Handlers.Account.Entries.ReceiptEntries;

public class ReceiptEntryCreateCommandHandler(IReceiptEntryService service) : ICommandHandler<ReceiptEntryCreateCommand, ApiResponse<Entry>>
{
    public async Task<ApiResponse<Entry>> Handle(ReceiptEntryCreateCommand request, CancellationToken cancellationToken)
    {
        return await service.Create(request);
    }
}