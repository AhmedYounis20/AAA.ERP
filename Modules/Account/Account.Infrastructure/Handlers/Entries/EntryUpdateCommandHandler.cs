using Account.Domain.Commands.Entries;

namespace Account.Infrastructure.Handlers.Entries;

public class EntryUpdateCommandHandler(IComplexEntryService service) : ICommandHandler<ComplexEntryUpdateCommand, ApiResponse<Entry>>
{
    public async Task<ApiResponse<Entry>> Handle(ComplexEntryUpdateCommand request, CancellationToken cancellationToken)
    {
        return await service.Update(request);
    }
}
