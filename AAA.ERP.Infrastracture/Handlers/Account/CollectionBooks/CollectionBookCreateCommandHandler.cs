using ERP.Application.Services.Account;
using ERP.Domain.Commands.Account.CollectionBooks;
using ERP.Domain.Models.Entities.Account.CollectionBooks;
using Shared;

namespace ERP.Infrastracture.Handlers.Account.CollectionBooks;

public class CollectionBookCreateCommandHandler(ICollectionBookService service) : ICommandHandler<CollectionBookCreateCommand, ApiResponse<CollectionBook>>
{
    public async Task<ApiResponse<CollectionBook>> Handle(CollectionBookCreateCommand request,
        CancellationToken cancellationToken)
    {
        return await service.Create(request);
    }
}