using Accounting.Application.Commands.ReceiptEntries;
using Accounting.Application.Services;
using ERP.Domain.Models.Entities.Account.Entries;

namespace Accounting.Infrastructure.Handlers.ReceiptEntries;

public class ReceiptEntryUpdateCommandHandler(IReceiptEntryService service)
    : ICommandHandler<ReceiptEntryUpdateCommand, ApiResponse<Entry>>
{
    public Task<ApiResponse<Entry>> Handle(ReceiptEntryUpdateCommand request, CancellationToken cancellationToken)
        => service.Update(request);
}
