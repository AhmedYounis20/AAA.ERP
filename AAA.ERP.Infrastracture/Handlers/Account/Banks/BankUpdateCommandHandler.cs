using ERP.Application.Services.Account.SubLeadgers;
using ERP.Domain.Commands.Account.SubLeadgers.Banks;
using ERP.Domain.Models.Entities.Account.SubLeadgers;
using Shared;

namespace ERP.Infrastracture.Handlers.Account.Banks;

public class BankUpdateCommandHandler(IBankService service) : ICommandHandler<BankUpdateCommand, ApiResponse<Bank>>
{
    public async Task<ApiResponse<Bank>> Handle(BankUpdateCommand request, CancellationToken cancellationToken)
    {
        return await service.Update(request);
    }
}