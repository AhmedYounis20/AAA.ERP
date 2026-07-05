using Accounting.Application.Services;
using Accounting.Application.Commands.Entries;
using ERP.Domain.Models.Entities.Account.Entries;

namespace Accounting.Infrastructure.Handlers.Entries;

public class EntryUpdateCommandHandler(IComplexEntryService service) : ICommandHandler<ComplexEntryUpdateCommand, ApiResponse<Entry>>
{
    public async Task<ApiResponse<Entry>> Handle(ComplexEntryUpdateCommand request, CancellationToken cancellationToken)
    {
        return await service.Update(request);
    }
}