using ERP.Application.Services.Account.SubLeadgers;
using ERP.Domain.Commands.Account.SubLeadgers.Banks;
using ERP.Domain.Models.Entities.Account.SubLeadgers;
using Shared;

namespace ERP.Infrastracture.Handlers.Account.Banks;

public class BankCreateCommandHandler(IBankService service) : ICommandHandler<BankCreateCommand, ApiResponse<Bank>>
{
    public async Task<ApiResponse<Bank>> Handle(BankCreateCommand request,
        CancellationToken cancellationToken)
    {
        return await service.Create(request);
    }
}