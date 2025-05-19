using Domain.Account.Commands.SubLeadgers.CashInBoxes;
using Domain.Account.Models.Entities.ChartOfAccounts;
using Domain.Account.Models.Entities.SubLeadgers;
using ERP.Application.Services.Account.SubLeadgers;
using ERP.Infrastracture.Services.Account.SubLeadgers.SubLeadgerBaseService;

namespace ERP.Infrastracture.Services.Account.SubLeadgers;

public class CashInBoxService : SubLeadgerService<CashInBox, CashInBoxCreateCommand, CashInBoxUpdateCommand>,
    ICashInBoxService
{
    private IUnitOfWork _unitOfWork;
    private IHttpContextAccessor _accessor;

    public CashInBoxService(IUnitOfWork unitOfWork, IHttpContextAccessor accessor)
        : base(unitOfWork, unitOfWork.CashInBoxRepository, accessor, SD.CashInBoxChartOfAccountId, SubLeadgerType.CashInBox)
    {
        _unitOfWork = unitOfWork;
        _accessor = accessor;
    }
}