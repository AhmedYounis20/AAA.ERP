using Accounting.Application.Commands.CollectionBooks;
using Accounting.Application.Services;
using ERP.Domain.Models.Entities.Account.CollectionBooks;

namespace Accounting.Infrastructure.Handlers.CollectionBooks;

public class CollectionBookUpdateCommandHandler(ICollectionBookService service)
    : ICommandHandler<CollectionBookUpdateCommand, ApiResponse<CollectionBook>>
{
    public Task<ApiResponse<CollectionBook>> Handle(CollectionBookUpdateCommand request, CancellationToken cancellationToken)
        => service.Update(request);
}
