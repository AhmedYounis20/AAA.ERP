using Domain.Account.Commands.AccountGuides;
using Domain.Account.Models.Entities.CollectionBooks;
using ERP.Application.Services.Account;
using Shared;

namespace ERP.Infrastracture.Handlers.CollectionBooks;

public class CollectionBookCreateCommandHandler(ICollectionBookService service) : ICommandHandler<CollectionBookCreateCommand, ApiResponse<CollectionBook>>
{
    public async Task<ApiResponse<CollectionBook>> Handle(CollectionBookCreateCommand request,
        CancellationToken cancellationToken)
    {
        return await service.Create(request);
    }
}