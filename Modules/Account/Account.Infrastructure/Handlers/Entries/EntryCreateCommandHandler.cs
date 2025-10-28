using Account.Domain.Commands.Entries;

namespace Account.Infrastructure.Handlers.Entries;

public class EntryCreateCommandHandler(IComplexEntryService service) : ICommandHandler<ComplexEntryCreateCommand, ApiResponse<Entry>>
{
    public async Task<ApiResponse<Entry>> Handle(ComplexEntryCreateCommand request,
        CancellationToken cancellationToken)
    {
        return await service.Create(request);
    }
}
