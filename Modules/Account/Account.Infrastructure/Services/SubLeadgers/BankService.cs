using Account.Domain.Commands.SubLeadgers.Banks;
using Account.Domain.Models.Entities.ChartOfAccounts;
using Account.Application.Repositories.SubLeadgers;
using Account.Application.Services.SubLeadgers;
using Account.Infrastructure.Services.SubLeadgers.SubLeadgerBaseService;

namespace Account.Infrastructure.Services.SubLeadgers;

public class BankService : SubLeadgerService<Bank, BankCreateCommand, BankUpdateCommand>,
    IBankService
{
    private IUnitOfWork _unitOfWork;

    public BankService(IUnitOfWork unitOfWork, IBankRepository repository)
        : base(unitOfWork, repository, SD.BankChartAccountId, SubLeadgerType.Bank)
    {
        _unitOfWork = unitOfWork;
    }
}
