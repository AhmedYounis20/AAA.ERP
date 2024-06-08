using AAA.ERP.Services.Interfaces.SubLeadgers;
using Domain.Account.Commands.Currencies;
using Domain.Account.InputModels.Subleadgers;
using Domain.Account.Models.Entities.Currencies;
using Domain.Account.Services.Interfaces;
using Shared;
using Shared.Responses;

namespace Domain.Account.Handlers.Currencies;

public class CurrencyCreateCommandHandler(ICurrencyService service): ICommandHandler<CurrencyCreateCommand,ApiResponse<Currency>>
{
    public async Task<ApiResponse<Currency>> Handle(CurrencyCreateCommand request,
        CancellationToken cancellationToken)
    {
        return await service.Create(request);
    }
}