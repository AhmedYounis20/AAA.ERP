using Accounting.Application.Commands.Banks;
using Accounting.Application.Services;
using Accounting.Domain.Models.Entities.SubLeadgers;

namespace Accounting.Infrastructure.Handlers.Banks;

public class BankDeleteCommandHandler(IBankService bankService)
    : ICommandHandler<BankDeleteCommand, ApiResponse<Bank>>
{
    public Task<ApiResponse<Bank>> Handle(BankDeleteCommand request, CancellationToken cancellationToken)
        => bankService.Delete(request.Id);
}
