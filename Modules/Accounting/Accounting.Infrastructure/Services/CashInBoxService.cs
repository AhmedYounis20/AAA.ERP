using Accounting.Application.Commands.CashInBoxes;
using Accounting.Application.Repositories;
using Accounting.Application.Services;
using Domain.Account.Utility;
using Accounting.Domain.Models.Entities.ChartOfAccounts;
using Accounting.Domain.Models.Entities.SubLeadgers;
using ERP.Infrastructure.Services.Account.SubLeadgers.SubLeadgerBaseService;

namespace Accounting.Infrastructure.Services;

public class CashInBoxService : SubLeadgerService<CashInBox, CashInBoxCreateCommand, CashInBoxUpdateCommand>, ICashInBoxService
{
    public CashInBoxService(IUnitOfWork unitOfWork, ICashInBoxRepository repository, IHttpContextAccessor accessor,
        IChartOfAccountRepository chartOfAccountRepository)
        : base(unitOfWork, repository, accessor, chartOfAccountRepository, SD.CashInBoxChartOfAccountId, SubLeadgerType.CashInBox)
    {
    }
}
