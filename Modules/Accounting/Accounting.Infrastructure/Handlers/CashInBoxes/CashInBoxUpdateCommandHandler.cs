using Accounting.Application.Commands.CashInBoxes;
using Accounting.Application.Services;
using ERP.Domain.Models.Entities.Account.SubLeadgers;

namespace Accounting.Infrastructure.Handlers.CashInBoxes;

public class CashInBoxUpdateCommandHandler(ICashInBoxService service)
    : ICommandHandler<CashInBoxUpdateCommand, ApiResponse<CashInBox>>
{
    public Task<ApiResponse<CashInBox>> Handle(CashInBoxUpdateCommand request, CancellationToken cancellationToken)
        => service.Update(request);
}
