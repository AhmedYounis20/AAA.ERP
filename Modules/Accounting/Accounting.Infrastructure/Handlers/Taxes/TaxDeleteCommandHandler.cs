using Accounting.Application.Commands.Taxes;
using Accounting.Application.Services;
using Accounting.Domain.Models.Entities.Account.SubLeadgers;

namespace Accounting.Infrastructure.Handlers.Taxes;

/// <summary>
/// Delete via MediatR. Transitional: delegates to <see cref="ITaxService"/> until delete logic is inlined.
/// </summary>
public class TaxDeleteCommandHandler(ITaxService taxService)
    : ICommandHandler<TaxDeleteCommand, ApiResponse<Tax>>
{
    public Task<ApiResponse<Tax>> Handle(TaxDeleteCommand request, CancellationToken cancellationToken)
        => taxService.Delete(request.Id);
}
