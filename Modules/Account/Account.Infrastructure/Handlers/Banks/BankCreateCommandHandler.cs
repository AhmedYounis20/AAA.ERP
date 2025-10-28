using Account.Application.Services.SubLeadgers;
using Account.Domain.Commands.SubLeadgers.Banks;
using Account.Domain.Models.Entities.SubLeadgers;
using Shared;

namespace Account.Infrastructure.Handlers.Banks;

public class BankCreateCommandHandler(IBankService service) : ICommandHandler<BankCreateCommand, ApiResponse<Bank>>
{
    public async Task<ApiResponse<Bank>> Handle(BankCreateCommand request,
        CancellationToken cancellationToken)
    {
        return await service.Create(request);
    }
}
