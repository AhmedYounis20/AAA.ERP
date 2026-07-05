using Accounting.Application.Commands.Taxes;
using Accounting.Application.Repositories;
using Accounting.Application.Services;
using Accounting.Domain.Entities;
using ERP.Application.Repositories;
using ERP.Domain.Models.Entities.Account.ChartOfAccounts;
using ERP.Domain.Models.Entities.Account.SubLeadgers;
using ERP.Infrastructure.Services.Account.SubLeadgers.SubLeadgerBaseService;
using Shared.BaseEntities;

namespace Accounting.Infrastructure.Services;

public class TaxService : SubLeadgerService<Tax, TaxCreateCommand, TaxUpdateCommand>, ITaxService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ITaxRepository _repository;
    private readonly IChartOfAccountRepository _chartOfAccountRepository;

    public TaxService(IUnitOfWork unitOfWork, ITaxRepository repository, IHttpContextAccessor accessor,
        IChartOfAccountRepository chartOfAccountRepository)
        : base(unitOfWork, repository, accessor, chartOfAccountRepository, string.Empty, SubLeadgerType.Tax)
    {
        _unitOfWork = unitOfWork;
        _repository = repository;
        _chartOfAccountRepository = chartOfAccountRepository;
    }

    public override async Task<ApiResponse<TaxCreateCommand>> GetNextSubLeadgers(Guid? parentId)
    {
        if (!parentId.HasValue)
        {
            return new ApiResponse<TaxCreateCommand>
            {
                IsSuccess = false,
                StatusCode = HttpStatusCode.BadRequest,
                Errors = [new MessageTemplate { MessageKey = "ParentIdIsRequired" }]
            };
        }

        var code = await _chartOfAccountRepository.GenerateNewCodeForChild(parentId);
        var inputModel = new TaxCreateCommand
        {
            Code = code,
            NodeType = NodeType.Domain,
            ParentId = parentId
        };

        return new ApiResponse<TaxCreateCommand>
        {
            IsSuccess = true,
            StatusCode = HttpStatusCode.OK,
            Result = inputModel
        };
    }

    public override async Task<ApiResponse<Tax>> Create(TaxCreateCommand command, bool isValidate = true)
    {
        try
        {
            var isDomain = command.NodeType.Equals(NodeType.Domain);
            var parentAccountId = command.ChartOfAccountId;

            var chartOfAccountParent = await _chartOfAccountRepository.Get(parentAccountId);
            var newCode = await _chartOfAccountRepository.GenerateNewCodeForChild(parentAccountId);

            var entity = command.Adapt<Tax>();
            entity.ChartOfAccountId = null;
            if (isDomain)
            {
                entity.ChartOfAccount = new ChartOfAccount
                {
                    Name = command.Name,
                    NameSecondLanguage = command.NameSecondLanguage,
                    ParentId = parentAccountId,
                    Code = newCode,
                    AccountNature = chartOfAccountParent?.AccountNature ?? AccountNature.Debit,
                    IsDepreciable = chartOfAccountParent?.IsDepreciable ?? false,
                    IsActiveAccount = chartOfAccountParent?.IsActiveAccount ?? false,
                    IsStopDealing = chartOfAccountParent?.IsStopDealing ?? true,
                    IsPostedAccount = chartOfAccountParent?.IsPostedAccount ?? false,
                    AccountGuidId = chartOfAccountParent?.AccountGuidId ?? Guid.NewGuid(),
                    IsCreatedFromSubLeadger = true,
                    SubLeadgerType = SubLeadgerType.Tax
                };
            }
            else
            {
                entity.ChartOfAccountId = null;
                entity.ChartOfAccount = null;
            }

            await _repository.Add(entity);

            return new ApiResponse<Tax>
            {
                IsSuccess = true,
                StatusCode = HttpStatusCode.OK,
                Result = entity,
            };
        }
        catch (Exception ex)
        {
            return new ApiResponse<Tax>
            {
                IsSuccess = false,
                Errors = [new MessageTemplate { MessageKey = ex.ToString() }]
            };
        }
    }
}
