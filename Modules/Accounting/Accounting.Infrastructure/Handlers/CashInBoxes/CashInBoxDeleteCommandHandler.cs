using Accounting.Application.Commands.CashInBoxes;
using Accounting.Application.Services;
using Accounting.Domain.Models.Entities.SubLeadgers;

namespace Accounting.Infrastructure.Handlers.CashInBoxes;

public class CashInBoxDeleteCommandHandler(ICashInBoxService service)
    : ICommandHandler<CashInBoxDeleteCommand, ApiResponse<CashInBox>>
{
    public Task<ApiResponse<CashInBox>> Handle(CashInBoxDeleteCommand request, CancellationToken cancellationToken)
        => service.Delete(request.Id);
}
