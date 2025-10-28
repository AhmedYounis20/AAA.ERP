using Account.Domain.Commands.CollectionBooks;
using Account.Domain.Models.Entities.CollectionBooks;

namespace Account.Infrastructure.Handlers.CollectionBooks;

public class CollectionBookCreateCommandHandler(ICollectionBookService service) : ICommandHandler<CollectionBookCreateCommand, ApiResponse<CollectionBook>>
{
    public async Task<ApiResponse<CollectionBook>> Handle(CollectionBookCreateCommand request,
        CancellationToken cancellationToken)
    {
        return await service.Create(request);
    }
}
