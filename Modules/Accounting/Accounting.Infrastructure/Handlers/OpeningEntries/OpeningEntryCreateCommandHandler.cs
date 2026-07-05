using Accounting.Application.Services;
using Accounting.Application.Commands.OpeningEntries;
using Accounting.Domain.Models.Entities.Entries;

namespace Accounting.Infrastructure.Handlers.OpeningEntries;

public class OpeningEntryCreateCommandHandler(IOpeningEntryService openingEntryService) : ICommandHandler<OpeningEntryCreateCommand, ApiResponse<Entry>>
{
    public async Task<ApiResponse<Entry>> Handle(OpeningEntryCreateCommand request, CancellationToken cancellationToken)
    {
        return await openingEntryService.Create(request);
    }
}