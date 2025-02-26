using AAA.ERP.Services.Interfaces.SubLeadgers;
using Domain.Account.Commands.SubLeadgers.CashInBoxes;
using Domain.Account.InputModels.Subleadgers;
using Domain.Account.Models.Entities.ChartOfAccounts;
using Domain.Account.Models.Entities.SubLeadgers;
using Domain.Account.Repositories.Interfaces;
using Domain.Account.Services.Impelementation.SubLeadgers.SubLeadgerBaseService;
using Domain.Account.Utility;
using Microsoft.AspNetCore.Http;
using Shared.BaseEntities;
using Shared.Responses;

namespace AAA.ERP.Services.Impelementation.SubLeadgers;

public class CashInBoxService : SubLeadgerService<CashInBox, CashInBoxCreateCommand, CashInBoxUpdateCommand>,
    ICashInBoxService
{
    private IUnitOfWork _unitOfWork;
    private IHttpContextAccessor _accessor;

    public CashInBoxService(IUnitOfWork unitOfWork, IHttpContextAccessor accessor)
        : base(unitOfWork, unitOfWork.CashInBoxRepository, accessor, SD.CashInBoxChartOfAccountId,SubLeadgerType.CashInBox)
    {
        _unitOfWork = unitOfWork;
        _accessor = accessor;
    }
}