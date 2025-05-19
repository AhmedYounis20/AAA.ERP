using ERP.Application.Services.Account.Entries;
using ERP.Domain.Commands.Account.Entries;
using ERP.Domain.Models.Entities.Account.Entries;

namespace ERP.Infrastracture.Handlers.Account.Entries;

public class EntryCreateCommandHandler(IComplexEntryService service) : ICommandHandler<ComplexEntryCreateCommand, ApiResponse<Entry>>
{
    public async Task<ApiResponse<Entry>> Handle(ComplexEntryCreateCommand request,
        CancellationToken cancellationToken)
    {
        return await service.Create(request);
    }
}