using Domain.Account.Commands.AccountGuides;
using Domain.Account.Models.Entities.CollectionBooks;
using ERP.Application.Services.Account;
using Shared;

namespace ERP.Infrastracture.Handlers.CollectionBooks;

public class CollectionBookUpdateCommandHandler(ICollectionBookService service) : ICommandHandler<CollectionBookUpdateCommand, ApiResponse<CollectionBook>>
{
    public async Task<ApiResponse<CollectionBook>> Handle(CollectionBookUpdateCommand request, CancellationToken cancellationToken)
    {
        return await service.Update(request);
    }
}