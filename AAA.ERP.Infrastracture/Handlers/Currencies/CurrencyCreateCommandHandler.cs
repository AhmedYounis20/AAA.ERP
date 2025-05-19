using Domain.Account.Commands.Currencies;
using Domain.Account.Models.Entities.Currencies;

namespace ERP.Infrastracture.Handlers.Currencies;

public class CurrencyCreateCommandHandler(ICurrencyService service) : ICommandHandler<CurrencyCreateCommand, ApiResponse<Currency>>
{
    public async Task<ApiResponse<Currency>> Handle(CurrencyCreateCommand request,
        CancellationToken cancellationToken)
    {
        return await service.Create(request);
    }
}