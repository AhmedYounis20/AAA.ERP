using Account.Domain.Commands.AccountGuides;
using Account.Domain.Models.Entities.AccountGuides;


namespace Account.Infrastructure.Handlers.AccountGuides;

public class AccountGuideCreateCommandHandler(IAccountGuideService service) : ICommandHandler<AccountGuideCreateCommand, ApiResponse<AccountGuide>>
{
    public async Task<ApiResponse<AccountGuide>> Handle(AccountGuideCreateCommand request,
        CancellationToken cancellationToken)
    {
        return await service.Create(request);
    }
}
