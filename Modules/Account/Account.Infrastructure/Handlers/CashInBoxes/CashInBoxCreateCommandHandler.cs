using Account.Application.Services.SubLeadgers;
using Account.Domain.Commands.SubLeadgers.CashInBoxes;
using Account.Domain.Models.Entities.SubLeadgers;
using Shared;

namespace Account.Infrastructure.Handlers.CashInBoxes;

public class CashInBoxCreateCommandHandler(ICashInBoxService service) : ICommandHandler<CashInBoxCreateCommand, ApiResponse<CashInBox>>
{
    public async Task<ApiResponse<CashInBox>> Handle(CashInBoxCreateCommand request,
        CancellationToken cancellationToken)
    {
        return await service.Create(request);
    }
}
