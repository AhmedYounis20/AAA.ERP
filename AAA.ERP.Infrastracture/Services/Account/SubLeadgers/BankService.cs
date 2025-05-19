using Domain.Account.Commands.SubLeadgers.Banks;
using Domain.Account.Models.Entities.ChartOfAccounts;
using Domain.Account.Models.Entities.SubLeadgers;
using ERP.Application.Repositories.SubLeadgers;
using ERP.Application.Services.Account.SubLeadgers;
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