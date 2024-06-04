using AAA.ERP.Services.Interfaces.SubLeadgers;
using Domain.Account.Commands.Currencies;
using Domain.Account.Commands.SubLeadgers.CashInBoxes;
using Domain.Account.InputModels.Subleadgers;
using Domain.Account.Models.Entities.Currencies;
using Shared;
using Shared.Responses;

namespace Domain.Account.Handlers.Currencies;

public class CurrencyUpdateCommandHandler(ICashInBoxService service): ICommandHandler<CurrencyUpdateCommand,ApiResponse<Currency>>
{
    public async Task<ApiResponse<Currency>> Handle(CurrencyUpdateCommand request, CancellationToken cancellationToken)
    {
         await service.Create(new BaseSubLeadgerInputModel());

         return new ApiResponse<Currency>();
    }
}