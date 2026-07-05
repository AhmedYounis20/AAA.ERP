using Accounting.Application.Commands.CollectionBooks;
using Accounting.Application.Services;
using Accounting.Domain.Models.Entities.CollectionBooks;

namespace Accounting.Infrastructure.Handlers.CollectionBooks;

public class CollectionBookDeleteCommandHandler(ICollectionBookService service)
    : ICommandHandler<CollectionBookDeleteCommand, ApiResponse<CollectionBook>>
{
    public Task<ApiResponse<CollectionBook>> Handle(CollectionBookDeleteCommand request, CancellationToken cancellationToken)
        => service.Delete(request.Id);
}
