using Accounting.Application.Commands.CashInBoxes;
using Accounting.Application.Services;
using Accounting.Domain.Models.Entities.SubLeadgers;

namespace Accounting.Infrastructure.Handlers.CashInBoxes;

public class CashInBoxCreateCommandHandler(ICashInBoxService service)
    : ICommandHandler<CashInBoxCreateCommand, ApiResponse<CashInBox>>
{
    public Task<ApiResponse<CashInBox>> Handle(CashInBoxCreateCommand request, CancellationToken cancellationToken)
        => service.Create(request);
}
