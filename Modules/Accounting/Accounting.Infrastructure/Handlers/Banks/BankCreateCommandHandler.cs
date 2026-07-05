using Accounting.Application.Commands.Banks;
using Accounting.Application.Services;
using ERP.Domain.Models.Entities.Account.SubLeadgers;

namespace Accounting.Infrastructure.Handlers.Banks;

public class BankCreateCommandHandler(IBankService service)
    : ICommandHandler<BankCreateCommand, ApiResponse<Bank>>
{
    public Task<ApiResponse<Bank>> Handle(BankCreateCommand request, CancellationToken cancellationToken)
        => service.Create(request);
}
