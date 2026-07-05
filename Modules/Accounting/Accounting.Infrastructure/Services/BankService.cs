using Accounting.Application.Commands.Banks;
using Accounting.Application.Repositories;
using Accounting.Application.Services;
using Domain.Account.Utility;
using Accounting.Domain.Models.Entities.ChartOfAccounts;
using Accounting.Domain.Models.Entities.SubLeadgers;
using ERP.Infrastructure.Services.Account.SubLeadgers.SubLeadgerBaseService;

namespace Accounting.Infrastructure.Services;

public class BankService : SubLeadgerService<Bank, BankCreateCommand, BankUpdateCommand>, IBankService
{
    public BankService(IUnitOfWork unitOfWork, IBankRepository repository, IHttpContextAccessor accessor,
        IChartOfAccountRepository chartOfAccountRepository)
        : base(unitOfWork, repository, accessor, chartOfAccountRepository, SD.BankChartAccountId, SubLeadgerType.Bank)
    {
    }
}
