using Domain.Account.Commands.SubLeadgers.Banks;
using Domain.Account.Models.Entities.SubLeadgers;
using ERP.Application.Services.Account.SubLeadgers;
using Shared;

namespace ERP.Infrastracture.Handlers.Banks;

public class BankCreateCommandHandler(IBankService service) : ICommandHandler<BankCreateCommand, ApiResponse<Bank>>
{
    public async Task<ApiResponse<Bank>> Handle(BankCreateCommand request,
        CancellationToken cancellationToken)
    {
        return await service.Create(request);
    }
}