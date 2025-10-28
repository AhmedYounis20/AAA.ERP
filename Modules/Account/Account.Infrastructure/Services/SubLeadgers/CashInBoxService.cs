using Account.Domain.Commands.SubLeadgers.CashInBoxes;
using Account.Domain.Models.Entities.ChartOfAccounts;

namespace Account.Infrastructure.Services.SubLeadgers;

public class CashInBoxService : SubLeadgerService<CashInBox, CashInBoxCreateCommand, CashInBoxUpdateCommand>,
    ICashInBoxService
{
    private IUnitOfWork _unitOfWork;

    public CashInBoxService(IUnitOfWork unitOfWork)
        : base(unitOfWork, unitOfWork.CashInBoxRepository, SD.CashInBoxChartOfAccountId, SubLeadgerType.CashInBox)
    {
        _unitOfWork = unitOfWork;
    }
}
