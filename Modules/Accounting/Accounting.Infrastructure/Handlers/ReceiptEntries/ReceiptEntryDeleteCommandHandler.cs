using Accounting.Application.Commands.ReceiptEntries;
using Accounting.Application.Services;
using Accounting.Domain.Models.Entities.Entries;

namespace Accounting.Infrastructure.Handlers.ReceiptEntries;

public class ReceiptEntryDeleteCommandHandler(IReceiptEntryService receiptEntryService)
    : ICommandHandler<ReceiptEntryDeleteCommand, ApiResponse<Entry>>
{
    public Task<ApiResponse<Entry>> Handle(ReceiptEntryDeleteCommand request, CancellationToken cancellationToken)
        => receiptEntryService.Delete(request.Id);
}
