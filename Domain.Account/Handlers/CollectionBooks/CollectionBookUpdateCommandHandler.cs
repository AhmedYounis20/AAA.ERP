using AAA.ERP.Services.Interfaces.SubLeadgers;
using Domain.Account.Commands.AccountGuides;
using Domain.Account.InputModels.Subleadgers;
using Domain.Account.Models.Entities.AccountGuide;
using Domain.Account.Models.Entities.CollectionBooks;
using Domain.Account.Services.Interfaces;
using Shared;
using Shared.Responses;

namespace Domain.Account.Handlers.CollectionBooks;

public class CollectionBookUpdateCommandHandler(ICollectionBookService service): ICommandHandler<CollectionBookUpdateCommand,ApiResponse<CollectionBook>>
{
    public async Task<ApiResponse<CollectionBook>> Handle(CollectionBookUpdateCommand request, CancellationToken cancellationToken)
    {
        return await service.Update(request);
    }
}