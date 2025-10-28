using Account.Domain.Commands.Entries.ReceiptEntries;

namespace Account.Infrastructure.Handlers.Entries.ReceiptEntries;

public class ReceiptEntryCreateCommandHandler(IReceiptEntryService service) : ICommandHandler<ReceiptEntryCreateCommand, ApiResponse<Entry>>
{
    public async Task<ApiResponse<Entry>> Handle(ReceiptEntryCreateCommand request, CancellationToken cancellationToken)
    {
        return await service.Create(request);
    }
}
