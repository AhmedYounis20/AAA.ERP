using Domain.Account.Commands.Entries;
using Domain.Account.Models.Entities.Entries;
using ERP.Application.Services.Account.Entries;

namespace ERP.Infrastracture.Handlers.Entries;

public class EntryCreateCommandHandler(IComplexEntryService service): ICommandHandler<ComplexEntryCreateCommand,ApiResponse<Entry>>
{
    public async Task<ApiResponse<Entry>> Handle(ComplexEntryCreateCommand request,
        CancellationToken cancellationToken)
    {
        return await service.Create(request);
    }
}