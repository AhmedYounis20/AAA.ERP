using Account.Domain.Commands.CollectionBooks;
using Account.Domain.Models.Entities.CollectionBooks;

namespace Account.Infrastructure.Handlers.CollectionBooks;

public class CollectionBookUpdateCommandHandler(ICollectionBookService service) : ICommandHandler<CollectionBookUpdateCommand, ApiResponse<CollectionBook>>
{
    public async Task<ApiResponse<CollectionBook>> Handle(CollectionBookUpdateCommand request, CancellationToken cancellationToken)
    {
        return await service.Update(request);
    }
}
