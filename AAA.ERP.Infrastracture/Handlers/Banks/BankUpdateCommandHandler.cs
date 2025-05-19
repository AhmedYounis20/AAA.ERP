using Domain.Account.Commands.SubLeadgers.Banks;
using Domain.Account.Models.Entities.SubLeadgers;
using ERP.Application.Services.Account.SubLeadgers;
using Shared;

namespace ERP.Infrastracture.Handlers.Banks;

public class BankUpdateCommandHandler(IBankService service) : ICommandHandler<BankUpdateCommand, ApiResponse<Bank>>
{
    public async Task<ApiResponse<Bank>> Handle(BankUpdateCommand request, CancellationToken cancellationToken)
    {
        return await service.Update(request);
    }
}