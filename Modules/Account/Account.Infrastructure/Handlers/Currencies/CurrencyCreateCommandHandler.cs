using Account.Domain.Commands.Currencies;
using Account.Domain.Models.Entities.Currencies;

namespace Account.Infrastructure.Handlers.Currencies;

public class CurrencyCreateCommandHandler(ICurrencyService service) : ICommandHandler<CurrencyCreateCommand, ApiResponse<Currency>>
{
    public async Task<ApiResponse<Currency>> Handle(CurrencyCreateCommand request,
        CancellationToken cancellationToken)
    {
        return await service.Create(request);
    }
}
