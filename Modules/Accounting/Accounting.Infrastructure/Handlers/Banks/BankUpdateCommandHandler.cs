using Accounting.Application.Commands.Banks;
using Accounting.Application.Services;
using Accounting.Domain.Models.Entities.SubLeadgers;

namespace Accounting.Infrastructure.Handlers.Banks;

public class BankUpdateCommandHandler(IBankService service)
    : ICommandHandler<BankUpdateCommand, ApiResponse<Bank>>
{
    public Task<ApiResponse<Bank>> Handle(BankUpdateCommand request, CancellationToken cancellationToken)
        => service.Update(request);
}
