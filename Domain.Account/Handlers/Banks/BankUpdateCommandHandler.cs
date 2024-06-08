using Domain.Account.Commands.SubLeadgers.Banks;
using Domain.Account.Models.Entities.SubLeadgers;
using Domain.Account.Services.Interfaces.SubLeadgers;
using Shared;
using Shared.Responses;

namespace Domain.Account.Handlers.Banks;

public class BankUpdateCommandHandler(IBankService service): ICommandHandler<BankUpdateCommand,ApiResponse<Bank>>
{
    public async Task<ApiResponse<Bank>> Handle(BankUpdateCommand request, CancellationToken cancellationToken)
    {
        return await service.Update(request);
    }
}