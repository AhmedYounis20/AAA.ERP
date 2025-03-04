using AAA.ERP.Services.Interfaces.SubLeadgers;
using Domain.Account.Commands.AccountGuides;
using Domain.Account.Commands.SubLeadgers.CashInBoxes;
using Domain.Account.InputModels.Subleadgers;
using Domain.Account.Models.Entities.AccountGuide;
using Domain.Account.Models.Entities.CollectionBooks;
using Domain.Account.Models.Entities.SubLeadgers;
using Domain.Account.Services.Interfaces;
using Shared;
using Shared.Responses;

namespace Domain.Account.Handlers.CollectionBooks;

public class CollectionBookCreateCommandHandler(ICollectionBookService service): ICommandHandler<CollectionBookCreateCommand,ApiResponse<CollectionBook>>
{
    public async Task<ApiResponse<CollectionBook>> Handle(CollectionBookCreateCommand request,
        CancellationToken cancellationToken)
    {
        return  await service.Create(request);
    }
}