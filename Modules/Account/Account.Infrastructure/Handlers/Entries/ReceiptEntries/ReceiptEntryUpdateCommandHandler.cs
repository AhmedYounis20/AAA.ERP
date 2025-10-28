using Account.Domain.Commands.Entries.ReceiptEntries;

namespace Account.Infrastructure.Handlers.Entries.ReceiptEntries;

public class ReceiptEntryUpdateCommandHandler(IReceiptEntryService service) : ICommandHandler<ReceiptEntryUpdateCommand, ApiResponse<Entry>>
{
    public async Task<ApiResponse<Entry>> Handle(ReceiptEntryUpdateCommand request, CancellationToken cancellationToken)
    {
        return await service.Update(request);
    }
}
