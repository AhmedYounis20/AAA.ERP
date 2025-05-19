using Domain.Account.Commands.Entries;
using Domain.Account.Models.Entities.Entries;
using ERP.Application.Services.Account.Entries;

namespace ERP.Infrastracture.Handlers.Entries;

public class EntryUpdateCommandHandler(IComplexEntryService service): ICommandHandler<ComplexEntryUpdateCommand,ApiResponse<Entry>>
{
    public async Task<ApiResponse<Entry>> Handle(ComplexEntryUpdateCommand request, CancellationToken cancellationToken)
    {
         return await service.Update(request);
    }
}