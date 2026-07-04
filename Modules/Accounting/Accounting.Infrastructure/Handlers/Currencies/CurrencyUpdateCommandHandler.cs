using Accounting.Application.Commands.Currencies;
using Accounting.Application.Services;
using ERP.Domain.Models.Entities.Account.Currencies;

namespace Accounting.Infrastructure.Handlers.Currencies;

public class CurrencyUpdateCommandHandler(ICurrencyService service)
    : ICommandHandler<CurrencyUpdateCommand, ApiResponse<Currency>>
{
    public Task<ApiResponse<Currency>> Handle(CurrencyUpdateCommand request, CancellationToken cancellationToken)
        => service.Update(request);
}
