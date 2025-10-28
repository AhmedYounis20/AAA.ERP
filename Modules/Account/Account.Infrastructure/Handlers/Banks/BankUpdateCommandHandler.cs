using Account.Application.Services.SubLeadgers;
using Account.Domain.Commands.SubLeadgers.Banks;
using Account.Domain.Models.Entities.SubLeadgers;

namespace Account.Infrastructure.Handlers.Banks;

public class BankUpdateCommandHandler(IBankService service) : ICommandHandler<BankUpdateCommand, ApiResponse<Bank>>
{
    public async Task<ApiResponse<Bank>> Handle(BankUpdateCommand request, CancellationToken cancellationToken)
    {
        return await service.Update(request);
    }
}
