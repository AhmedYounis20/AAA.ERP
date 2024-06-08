using AAA.ERP.Services.Interfaces.SubLeadgers;
using Domain.Account.Commands.SubLeadgers.Banks;
using Domain.Account.Commands.SubLeadgers.CashInBoxes;
using Domain.Account.InputModels.Subleadgers;
using Domain.Account.Models.Entities.ChartOfAccounts;
using Domain.Account.Models.Entities.SubLeadgers;
using Domain.Account.Repositories.Interfaces;
using Domain.Account.Repositories.Interfaces.SubLeadgers;
using Domain.Account.Services.Impelementation.SubLeadgers.SubLeadgerBaseService;
using Domain.Account.Services.Interfaces.SubLeadgers;
using Domain.Account.Utility;
using Microsoft.AspNetCore.Http;
using Shared.BaseEntities;
using Shared.Responses;

namespace AAA.ERP.Services.Impelementation.SubLeadgers;

public class BankService : SubLeadgerService<Bank, BankCreateCommand,BankUpdateCommand>,
    IBankService
{
    private IUnitOfWork _unitOfWork;
    private IHttpContextAccessor _accessor;

    public BankService(IUnitOfWork unitOfWork,IBankRepository repository, IHttpContextAccessor accessor)
        : base(unitOfWork, repository, accessor,SD.BankChartAccountId)
    {
        _unitOfWork = unitOfWork;
        _accessor = accessor;
    }
}