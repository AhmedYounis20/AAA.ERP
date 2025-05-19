using Domain.Account.Commands.Currencies;
using Domain.Account.Models.Entities.Currencies;

namespace ERP.Infrastracture.Handlers.Currencies;

public class CurrencyUpdateCommandHandler(ICurrencyService service) : ICommandHandler<CurrencyUpdateCommand, ApiResponse<Currency>>
{
    public async Task<ApiResponse<Currency>> Handle(CurrencyUpdateCommand request, CancellationToken cancellationToken)
    {
        return await service.Update(request);
    }
}