using ERP.Domain.Commands.Account.Currencies;
using ERP.Domain.Models.Entities.Account.Currencies;

namespace ERP.Infrastracture.Handlers.Account.Currencies;

public class CurrencyUpdateCommandHandler(ICurrencyService service) : ICommandHandler<CurrencyUpdateCommand, ApiResponse<Currency>>
{
    public async Task<ApiResponse<Currency>> Handle(CurrencyUpdateCommand request, CancellationToken cancellationToken)
    {
        return await service.Update(request);
    }
}