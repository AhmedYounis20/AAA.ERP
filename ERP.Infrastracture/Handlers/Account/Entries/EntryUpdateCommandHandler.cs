using ERP.Application.Services.Account.Entries;
using ERP.Domain.Commands.Account.Entries;
using ERP.Domain.Models.Entities.Account.Entries;

namespace ERP.Infrastracture.Handlers.Account.Entries;

public class EntryUpdateCommandHandler(IComplexEntryService service) : ICommandHandler<ComplexEntryUpdateCommand, ApiResponse<Entry>>
{
    public async Task<ApiResponse<Entry>> Handle(ComplexEntryUpdateCommand request, CancellationToken cancellationToken)
    {
        return await service.Update(request);
    }
}