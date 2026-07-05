using Accounting.Application.Commands.Currencies;
using Accounting.Application.Services;
using Accounting.Domain.Models.Entities.Currencies;

namespace Accounting.Infrastructure.Handlers.Currencies;

public class CurrencyDeleteCommandHandler(ICurrencyService service)
    : ICommandHandler<CurrencyDeleteCommand, ApiResponse<Currency>>
{
    public Task<ApiResponse<Currency>> Handle(CurrencyDeleteCommand request, CancellationToken cancellationToken)
        => service.Delete(request.Id);
}
