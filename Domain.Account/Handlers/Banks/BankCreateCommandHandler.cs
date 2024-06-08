using Domain.Account.Commands.SubLeadgers.Banks;
using Domain.Account.Commands.SubLeadgers.CashInBoxes;
using Domain.Account.Models.Entities.SubLeadgers;
using Domain.Account.Services.Interfaces.SubLeadgers;
using Shared;
using Shared.Responses;

namespace Domain.Account.Handlers.Banks;

public class BankCreateCommandHandler(IBankService service): ICommandHandler<BankCreateCommand,ApiResponse<Bank>>
{
    public async Task<ApiResponse<Bank>> Handle(BankCreateCommand request,
        CancellationToken cancellationToken)
    {
        return await service.Create(request);
    }
}