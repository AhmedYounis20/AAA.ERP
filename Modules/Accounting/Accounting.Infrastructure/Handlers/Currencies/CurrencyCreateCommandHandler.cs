using Accounting.Application.Commands.Currencies;
using Accounting.Application.Repositories;
using Accounting.Application.Services;
using ERP.Domain.Models.Entities.Account.Currencies;

namespace Accounting.Infrastructure.Handlers.Currencies;

public class CurrencyCreateCommandHandler(ICurrencyService service)
    : ICommandHandler<CurrencyCreateCommand, ApiResponse<Currency>>
{
    public Task<ApiResponse<Currency>> Handle(CurrencyCreateCommand request, CancellationToken cancellationToken)
        => service.Create(request);
}
