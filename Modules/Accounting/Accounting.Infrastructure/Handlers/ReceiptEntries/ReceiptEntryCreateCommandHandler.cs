using Accounting.Application.Commands.ReceiptEntries;
using Accounting.Application.Services;
using ERP.Domain.Models.Entities.Account.Entries;

namespace Accounting.Infrastructure.Handlers.ReceiptEntries;

public class ReceiptEntryCreateCommandHandler(IReceiptEntryService service)
    : ICommandHandler<ReceiptEntryCreateCommand, ApiResponse<Entry>>
{
    public Task<ApiResponse<Entry>> Handle(ReceiptEntryCreateCommand request, CancellationToken cancellationToken)
        => service.Create(request);
}
