using ERP.Application.Repositories.Account.SubLeadgers;
using ERP.Application.Services.Account.SubLeadgers;
using ERP.Domain.Commands.Account.SubLeadgers.Taxes;
using ERP.Domain.Models.Entities.Account.ChartOfAccounts;
using ERP.Domain.Models.Entities.Account.SubLeadgers;
using ERP.Infrastracture.Services.Account.SubLeadgers.SubLeadgerBaseService;

namespace ERP.Infrastracture.Services.Account.SubLeadgers;

public class TaxService : SubLeadgerService<Tax, TaxCreateCommand, TaxUpdateCommand>,
    ITaxService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ITaxRepository _repository;

    public TaxService(IUnitOfWork unitOfWork, ITaxRepository repository, IHttpContextAccessor accessor)
        : base(unitOfWork, repository, accessor, string.Empty, SubLeadgerType.Tax)
    {
        _unitOfWork = unitOfWork;
        _repository = repository;
    }

    public override async Task<ApiResponse<TaxCreateCommand>> GetNextSubLeadgers(Guid? parentId)
    {
        if (!parentId.HasValue)
        {
            return new ApiResponse<TaxCreateCommand>
            {
                IsSuccess = false,
                StatusCode = HttpStatusCode.BadRequest,
                Errors = new List<MessageTemplate> { new MessageTemplate { MessageKey = "ParentIdIsRequired" } }
            };
        }

        string? code = await _unitOfWork.ChartOfAccountRepository.GenerateNewCodeForChild(parentId);
        TaxCreateCommand inputModel = new TaxCreateCommand
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
            bool isDomain = command.NodeType.Equals(NodeType.Domain);
            Guid parentAccountId = command.ChartOfAccountId;



            ChartOfAccount? chartOfAccountParent = await _unitOfWork.ChartOfAccountRepository.Get(parentAccountId);


            string newCode = await _unitOfWork.ChartOfAccountRepository.GenerateNewCodeForChild(parentAccountId);
            

            Tax entity = command.Adapt<Tax>();
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
                Errors = new List<MessageTemplate> { new MessageTemplate { MessageKey = ex.ToString() } }
            };
        }
    }
}
