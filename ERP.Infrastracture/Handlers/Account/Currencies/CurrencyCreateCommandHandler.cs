using ERP.Domain.Commands.Account.Currencies;
using ERP.Domain.Models.Entities.Account.Currencies;

namespace ERP.Infrastracture.Handlers.Account.Currencies;

public class CurrencyCreateCommandHandler(ICurrencyService service) : ICommandHandler<CurrencyCreateCommand, ApiResponse<Currency>>
{
    public async Task<ApiResponse<Currency>> Handle(CurrencyCreateCommand request,
        CancellationToken cancellationToken)
    {
        return await service.Create(request);
    }
}