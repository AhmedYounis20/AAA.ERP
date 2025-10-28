using Account.Domain.Commands.Entries.CompinedEntries;

namespace Account.Infrastructure.Handlers.Entries.CompinedEntries;

public class CompinedEntryCreateCommandHandler(ICompinedEntryService service) : ICommandHandler<CompinedEntryCreateCommand, ApiResponse<Entry>>
{
    public async Task<ApiResponse<Entry>> Handle(CompinedEntryCreateCommand request, CancellationToken cancellationToken)
    {
        return await service.Create(request);
    }
}
