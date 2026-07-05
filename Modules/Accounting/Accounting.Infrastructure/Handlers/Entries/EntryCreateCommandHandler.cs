using Accounting.Application.Services;
using Accounting.Application.Commands.Entries;
using Accounting.Domain.Models.Entities.Entries;

namespace Accounting.Infrastructure.Handlers.Entries;

public class EntryCreateCommandHandler(IComplexEntryService service) : ICommandHandler<ComplexEntryCreateCommand, ApiResponse<Entry>>
{
    public async Task<ApiResponse<Entry>> Handle(ComplexEntryCreateCommand request,
        CancellationToken cancellationToken)
    {
        return await service.Create(request);
    }
}