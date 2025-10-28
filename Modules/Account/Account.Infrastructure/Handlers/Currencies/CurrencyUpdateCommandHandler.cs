using Account.Domain.Commands.Currencies;
using Account.Domain.Models.Entities.Currencies;

namespace Account.Infrastructure.Handlers.Currencies;

public class CurrencyUpdateCommandHandler(ICurrencyService service) : ICommandHandler<CurrencyUpdateCommand, ApiResponse<Currency>>
{
    public async Task<ApiResponse<Currency>> Handle(CurrencyUpdateCommand request, CancellationToken cancellationToken)
    {
        return await service.Update(request);
    }
}
