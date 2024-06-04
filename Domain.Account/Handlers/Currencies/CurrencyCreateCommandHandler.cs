using AAA.ERP.Services.Interfaces.SubLeadgers;
using Domain.Account.Commands.Currencies;
using Domain.Account.InputModels.Subleadgers;
using Domain.Account.Models.Entities.Currencies;
using Shared;
using Shared.Responses;

namespace Domain.Account.Handlers.Currencies;

public class CurrencyCreateCommandHandler(ICashInBoxService service): ICommandHandler<CurrencyCreateCommand,ApiResponse<Currency>>
{
    public async Task<ApiResponse<Currency>> Handle(CurrencyCreateCommand request,
        CancellationToken cancellationToken)
    {
        await service.Create(new BaseSubLeadgerInputModel());

        return new ApiResponse<Currency>();
    }
}