using Account.Application.Services.Entries;
using Account.Domain.Commands.Entries.CompinedEntries;

namespace Account.Infrastructure.Handlers.Entries.CompinedEntries;

public class CompinedEntryUpdateCommandHandler(ICompinedEntryService service) : ICommandHandler<CompinedEntryUpdateCommand, ApiResponse<Entry>>
{
    public async Task<ApiResponse<Entry>> Handle(CompinedEntryUpdateCommand request, CancellationToken cancellationToken)
    {
        return await service.Update(request);
    }
}
