using Accounting.Application.Services;
using Accounting.Application.Commands.Entries;
using ERP.Domain.Models.Entities.Account.Entries;

namespace Accounting.Infrastructure.Handlers.Entries;

public class EntryCreateCommandHandler(IComplexEntryService service) : ICommandHandler<ComplexEntryCreateCommand, ApiResponse<Entry>>
{
    public async Task<ApiResponse<Entry>> Handle(ComplexEntryCreateCommand request,
        CancellationToken cancellationToken)
    {
        return await service.Create(request);
    }
}