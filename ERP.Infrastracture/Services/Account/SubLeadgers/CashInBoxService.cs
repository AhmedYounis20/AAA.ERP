using ERP.Application.Services.Account.SubLeadgers;
using ERP.Domain.Commands.Account.SubLeadgers.CashInBoxes;
using ERP.Domain.Models.Entities.Account.ChartOfAccounts;
using ERP.Domain.Models.Entities.Account.SubLeadgers;
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