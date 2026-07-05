using Accounting.Application.Commands.CollectionBooks;
using Accounting.Application.Services;
using Accounting.Domain.Models.Entities.CollectionBooks;

namespace Accounting.Infrastructure.Handlers.CollectionBooks;

public class CollectionBookCreateCommandHandler(ICollectionBookService service)
    : ICommandHandler<CollectionBookCreateCommand, ApiResponse<CollectionBook>>
{
    public Task<ApiResponse<CollectionBook>> Handle(CollectionBookCreateCommand request, CancellationToken cancellationToken)
        => service.Create(request);
}
