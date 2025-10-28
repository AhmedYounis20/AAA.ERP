using Account.Application.Services.SubLeadgers;
using Account.Domain.Commands.SubLeadgers.CashInBoxes;
using Account.Domain.Models.Entities.SubLeadgers;
using Shared;

namespace Account.Infrastructure.Handlers.CashInBoxes;

public class CashInBoxUpdateCommandHandler(ICashInBoxService service) : ICommandHandler<CashInBoxUpdateCommand, ApiResponse<CashInBox>>
{
    public async Task<ApiResponse<CashInBox>> Handle(CashInBoxUpdateCommand request, CancellationToken cancellationToken)
    {
        return await service.Update(request);
    }
}
