using ERP.Application.Services.Account;
using ERP.Domain.Commands.Account.CollectionBooks;
using ERP.Domain.Models.Entities.Account.CollectionBooks;
using Shared;

namespace ERP.Infrastracture.Handlers.Account.CollectionBooks;

public class CollectionBookUpdateCommandHandler(ICollectionBookService service) : ICommandHandler<CollectionBookUpdateCommand, ApiResponse<CollectionBook>>
{
    public async Task<ApiResponse<CollectionBook>> Handle(CollectionBookUpdateCommand request, CancellationToken cancellationToken)
    {
        return await service.Update(request);
    }
}