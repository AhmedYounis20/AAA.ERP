using Accounting.Application.Commands.CollectionBooks;
using Accounting.Application.Services;
using Accounting.Domain.Models.Entities.CollectionBooks;

namespace Accounting.Infrastructure.Handlers.CollectionBooks;

public class CollectionBookUpdateCommandHandler(ICollectionBookService service)
    : ICommandHandler<CollectionBookUpdateCommand, ApiResponse<CollectionBook>>
{
    public Task<ApiResponse<CollectionBook>> Handle(CollectionBookUpdateCommand request, CancellationToken cancellationToken)
        => service.Update(request);
}
