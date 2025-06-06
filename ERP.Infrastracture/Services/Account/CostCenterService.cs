﻿using ERP.Application.Repositories.Account;
using ERP.Application.Services.Account;
using ERP.Domain.Commands.Account.CostCenters;
using ERP.Domain.Models.Entities.Account.CostCenters;

namespace ERP.Infrastracture.Services.Account;

public class CostCenterService : BaseTreeSettingService<CostCenter, CostCenterCreateCommand, CostCenterUpdateCommand>,
    ICostCenterService
{
    private readonly ICostCenterRepository _repository;
    private readonly IChartOfAccountRepository _chartOfAccountRepository;
    private readonly IUnitOfWork _unitOfWork;

    public CostCenterService(ICostCenterRepository repository, IChartOfAccountRepository chartOfAccountRepository,
        IUnitOfWork unitOfWork) :
        base(repository)
    {
        _repository = repository;
        _chartOfAccountRepository = chartOfAccountRepository;
        _unitOfWork = unitOfWork;
    }

    public override async Task<ApiResponse<CostCenter>> Create(CostCenterCreateCommand command, bool isValidate = true)
    {
        try
        {
            var validationResult = await ValidateCreate(command);

            if (!validationResult.isValid)
            {
                return new ApiResponse<CostCenter>()
                {
                    IsSuccess = false,
                    StatusCode = HttpStatusCode.BadRequest,
                    ErrorMessages = validationResult.errors
                };
            }

            CostCenter costCenter = command.Adapt<CostCenter>();

            if (command.NodeType.Equals(NodeType.Domain) &&
                command.CostCenterType.Equals(CostCenterType.RelatedToAccount) && command.ChartOfAccounts != null)
            {
                costCenter.ChartOfAccounts = new List<CostCenterChartOfAccount>();
                foreach (var account in command.ChartOfAccounts)
                {
                    var accountINDb = await _chartOfAccountRepository.Get(account);
                    if (accountINDb != null)
                    {
                        costCenter.ChartOfAccounts.Add(new CostCenterChartOfAccount
                        {
                            ChartOfAccountId = account,
                            CreatedAt = DateTime.Now,
                            ModifiedAt = DateTime.Now,
                        });
                    }
                }
            }
            else
                costCenter.ChartOfAccounts = null;

            costCenter.CreatedAt = DateTime.Now;
            costCenter.ModifiedAt = DateTime.Now;

            var result = await _repository.Add(costCenter);
            return new ApiResponse<CostCenter>
            {
                IsSuccess = true,
                StatusCode = HttpStatusCode.OK,
                Result = result
            };
        }
        catch (Exception ex)
        {
            Log.Error(ex.ToString());
            return new ApiResponse<CostCenter>
            {
                IsSuccess = false,
                StatusCode = HttpStatusCode.BadRequest,
                ErrorMessages = [ex.Message.ToString()]
            };
        }
    }

    protected override async Task<(bool isValid, List<string> errors)> ValidateCreate(CostCenterCreateCommand command)
    {
        var result = await base.ValidateCreate(command);
        var accounts = await _chartOfAccountRepository.Get(e => command.ChartOfAccounts.Contains(e.Id));
        if (accounts.Count() != command.ChartOfAccounts.Count)
        {
            result.isValid = false;
            result.errors.Add("SomeChartOfAcountsNotExisted");
        }

        return result;
    }

    public override async Task<ApiResponse<CostCenter>> Update(CostCenterUpdateCommand command, bool isValidate = true)
    {
        await _unitOfWork.BeginTransactionAsync();
        try
        {
            var validationResult = await ValidateUpdate(command);

            if (!validationResult.isValid)
            {
                return new ApiResponse<CostCenter>
                {
                    IsSuccess = false,
                    StatusCode = HttpStatusCode.BadRequest,
                    ErrorMessages = validationResult.errors
                };
            }

            // get old entity from database 
            var oldCostCenter = validationResult.entity;
            oldCostCenter.Name = command.Name;
            oldCostCenter.NameSecondLanguage = command.NameSecondLanguage;
            if (oldCostCenter.NodeType.Equals(NodeType.Domain))
            {
                oldCostCenter.CostCenterType = command.CostCenterType;
                oldCostCenter.Percent = command.Percent;
                if (oldCostCenter.CostCenterType.Equals(CostCenterType.RelatedToAccount))
                {
                    // delete removed costCenterChartOfAccounts
                    List<CostCenterChartOfAccount> chartOfAccountsToRemove =
                        oldCostCenter.ChartOfAccounts.Where(e => command.ChartOfAccounts == null || !command.ChartOfAccounts.Contains(e.ChartOfAccountId)).ToList();
                    oldCostCenter.ChartOfAccounts = oldCostCenter.ChartOfAccounts
                        .Where(e => !chartOfAccountsToRemove.Contains(e)).ToList();
                    _repository.RemoveChartOfAccounts(chartOfAccountsToRemove);
                    // add new costCenterChartOfAccounts
                    List<Guid> chartOfAccountsToAdd =
                        command.ChartOfAccounts == null ? [] : command.ChartOfAccounts.Except(oldCostCenter.ChartOfAccounts.Select(e => e.ChartOfAccountId)).ToList();
                    chartOfAccountsToAdd.ForEach(e =>
                    {
                        oldCostCenter.ChartOfAccounts.Add(new CostCenterChartOfAccount
                        {
                            ChartOfAccountId = e,
                            CreatedAt = DateTime.Now,
                            ModifiedAt = DateTime.Now
                        });
                    });
                }
            }

            await _repository.Update(oldCostCenter);
            await _unitOfWork.CommitAsync();
            return new ApiResponse<CostCenter>
            {
                IsSuccess = true,
                StatusCode = HttpStatusCode.OK,
            };
        }
        catch (Exception ex)
        {
            Log.Error(ex.ToString());
            await _unitOfWork.RollbackAsync();
            return new ApiResponse<CostCenter>
            {
                IsSuccess = false,
                StatusCode = HttpStatusCode.BadRequest,
                ErrorMessages = [ex.Message.ToString()]
            };
        }
    }

    protected override async Task<(bool isValid, List<string> errors, CostCenter? entity)> ValidateUpdate(
        CostCenterUpdateCommand command)
    {
        var result = await base.ValidateUpdate(command);
        var oldCostCenter = await _repository.Get(command.Id);
        var chartOfAccountsExists = await _chartOfAccountRepository.Get(e => command.ChartOfAccounts != null && command.ChartOfAccounts.Contains(e.Id));
        if (command.ChartOfAccounts != null && chartOfAccountsExists.Count() != command.ChartOfAccounts.Count())
        {
            result.isValid = false;
            result.errors.Add("SomeChartOfAcountsNotExisted");
        }

        if (result.entity.NodeType != command.NodeType)
        {
            result.isValid = false;
            result.errors.Add("CannotChangeNodeType");
        }

        result.entity = oldCostCenter;
        return result;
    }
}