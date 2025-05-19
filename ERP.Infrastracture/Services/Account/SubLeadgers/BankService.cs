using ERP.Application.Repositories.Account.SubLeadgers;
using ERP.Application.Services.Account.SubLeadgers;
using ERP.Domain.Commands.Account.SubLeadgers.Banks;
using ERP.Domain.Models.Entities.Account.ChartOfAccounts;
using ERP.Domain.Models.Entities.Account.SubLeadgers;
using ERP.Infrastracture.Services.Account.SubLeadgers.SubLeadgerBaseService;

namespace ERP.Infrastracture.Services.Account.SubLeadgers;

public class BankService : SubLeadgerService<Bank, BankCreateCommand, BankUpdateCommand>,
    IBankService
{
    private IUnitOfWork _unitOfWork;
    private IHttpContextAccessor _accessor;

    public BankService(IUnitOfWork unitOfWork, IBankRepository repository, IHttpContextAccessor accessor)
        : base(unitOfWork, repository, accessor, SD.BankChartAccountId, SubLeadgerType.Bank)
    {
        _unitOfWork = unitOfWork;
        _accessor = accessor;
    }
}