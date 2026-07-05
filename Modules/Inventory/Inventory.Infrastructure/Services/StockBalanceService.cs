using Accounting.Contracts.Integration;
using ERP.BuildingBlocks.Application.Repositories.BaseRepositories;
using Inventory.Application.Repositories;
using ERP.Infrastructure.Services.BaseServices;
using Inventory.Application.Commands.StockBalances;
using Inventory.Application.Services;
using Inventory.Domain.Models.Dtos.Inventory;
using Inventory.Domain.Models.Entities.Sizes;
using Inventory.Domain.Models.Entities.Variants;
using Inventory.Infrastructure.Mapping;
using Shared.Constants;

namespace Inventory.Infrastructure.Services;

public class StockBalanceService : BaseService<StockBalance, StockBalanceCreateCommand, StockBalanceUpdateCommand>, IStockBalanceService
{
    private readonly IStockBalanceRepository _repository;
    private readonly IVariantRepository _variantRepository;
    private readonly IBaseRepository<VariantPackingUnit> _variantPackingUnitRepository;
    private readonly IInventoryAccountingDisplayQueries _displayQueries;

    public StockBalanceService(
        IStockBalanceRepository repository,
        IVariantRepository variantRepository,
        IBaseRepository<VariantPackingUnit> variantPackingUnitRepository,
        IInventoryAccountingDisplayQueries displayQueries) : base(repository)
    {
        _repository = repository;
        _variantRepository = variantRepository;
        _variantPackingUnitRepository = variantPackingUnitRepository;
        _displayQueries = displayQueries;
    }

    public async Task<ApiResponse<StockBalanceReadDto>> GetDisplayById(Guid id)
    {
        var balance = await _repository.Get(id);
        if (balance is null)
        {
            return new ApiResponse<StockBalanceReadDto>
            {
                IsSuccess = false,
                StatusCode = HttpStatusCode.NotFound,
                Errors = [new MessageTemplate { MessageKey = ErrorMessages.RecordNotFound }]
            };
        }

        var dto = await InventoryReadDtoMapper.ToReadDtoAsync(balance, _displayQueries);
        return new ApiResponse<StockBalanceReadDto> { IsSuccess = true, StatusCode = HttpStatusCode.OK, Result = dto };
    }

    public async Task<ApiResponse<IEnumerable<StockBalanceReadDto>>> GetAllForDisplay()
    {
        var balances = await _repository.Get();
        var dtos = await InventoryReadDtoMapper.ToReadDtosAsync(balances, _displayQueries);
        return new ApiResponse<IEnumerable<StockBalanceReadDto>> { IsSuccess = true, StatusCode = HttpStatusCode.OK, Result = dtos };
    }

    public async Task<ApiResponse<StockBalanceReadDto>> GetByVariantAndBranch(Guid variantId, Guid branchId)
    {
        try
        {
            var stockBalance = await _repository.GetByVariantAndBranch(variantId, branchId);
            if (stockBalance is null)
            {
                return new ApiResponse<StockBalanceReadDto>
                {
                    IsSuccess = false,
                    StatusCode = HttpStatusCode.NotFound,
                    Errors = [new MessageTemplate { MessageKey = ErrorMessages.RecordNotFound }]
                };
            }

            var dto = await InventoryReadDtoMapper.ToReadDtoAsync(stockBalance, _displayQueries);
            return new ApiResponse<StockBalanceReadDto> { IsSuccess = true, StatusCode = HttpStatusCode.OK, Result = dto };
        }
        catch (Exception ex)
        {
            return new ApiResponse<StockBalanceReadDto>
            {
                IsSuccess = false,
                StatusCode = HttpStatusCode.BadRequest,
                Errors = [new MessageTemplate { MessageKey = ex.Message }]
            };
        }
    }

    public async Task<ApiResponse<StockBalanceReadDto>> GetByVariantPackingUnitAndBranch(Guid variantId, Guid packingUnitId, Guid branchId)
    {
        try
        {
            var stockBalance = await _repository.GetByVariantPackingUnitAndBranch(variantId, packingUnitId, branchId);
            if (stockBalance is null)
            {
                return new ApiResponse<StockBalanceReadDto>
                {
                    IsSuccess = false,
                    StatusCode = HttpStatusCode.NotFound,
                    Errors = [new MessageTemplate { MessageKey = ErrorMessages.RecordNotFound }]
                };
            }

            var dto = await InventoryReadDtoMapper.ToReadDtoAsync(stockBalance, _displayQueries);
            return new ApiResponse<StockBalanceReadDto> { IsSuccess = true, StatusCode = HttpStatusCode.OK, Result = dto };
        }
        catch (Exception ex)
        {
            return new ApiResponse<StockBalanceReadDto>
            {
                IsSuccess = false,
                StatusCode = HttpStatusCode.BadRequest,
                Errors = [new MessageTemplate { MessageKey = ex.Message }]
            };
        }
    }

    public async Task<ApiResponse<IEnumerable<StockBalanceReadDto>>> GetByBranch(Guid branchId)
    {
        try
        {
            var stockBalances = await _repository.GetByBranch(branchId);
            var dtos = await InventoryReadDtoMapper.ToReadDtosAsync(stockBalances, _displayQueries);
            return new ApiResponse<IEnumerable<StockBalanceReadDto>> { IsSuccess = true, StatusCode = HttpStatusCode.OK, Result = dtos };
        }
        catch (Exception ex)
        {
            return new ApiResponse<IEnumerable<StockBalanceReadDto>>
            {
                IsSuccess = false,
                StatusCode = HttpStatusCode.BadRequest,
                Errors = [new MessageTemplate { MessageKey = ex.Message }]
            };
        }
    }

    public async Task<ApiResponse<IEnumerable<StockBalanceReadDto>>> GetByVariant(Guid variantId)
    {
        try
        {
            var stockBalances = await _repository.GetByVariant(variantId);
            var dtos = await InventoryReadDtoMapper.ToReadDtosAsync(stockBalances, _displayQueries);
            return new ApiResponse<IEnumerable<StockBalanceReadDto>> { IsSuccess = true, StatusCode = HttpStatusCode.OK, Result = dtos };
        }
        catch (Exception ex)
        {
            return new ApiResponse<IEnumerable<StockBalanceReadDto>>
            {
                IsSuccess = false,
                StatusCode = HttpStatusCode.BadRequest,
                Errors = [new MessageTemplate { MessageKey = ex.Message }]
            };
        }
    }

    public async Task<ApiResponse<decimal>> GetCurrentBalance(Guid variantId, Guid packingUnitId, Guid branchId)
    {
        try
        {
            var currentBalance = await _repository.GetCurrentBalance(variantId, packingUnitId, branchId);
            return new ApiResponse<decimal>
            {
                IsSuccess = true,
                StatusCode = HttpStatusCode.OK,
                Result = currentBalance
            };
        }
        catch (Exception ex)
        {
            return new ApiResponse<decimal>
            {
                IsSuccess = false,
                StatusCode = HttpStatusCode.BadRequest,
                Errors = new List<MessageTemplate> { new MessageTemplate { MessageKey = ex.Message } }
            };
        }
    }

    public async Task<ApiResponse<bool>> UpdateStockBalance(Guid variantId, Guid packingUnitId, Guid branchId, decimal quantity, decimal unitCost, bool isReceipt)
    {
        try
        {
            // Get the variant to find its default packing unit
            var variant = await _variantRepository.Get(variantId);
            if (variant == null)
            {
                return new ApiResponse<bool>
                {
                    IsSuccess = false,
                    StatusCode = HttpStatusCode.BadRequest,
                    Errors = new List<MessageTemplate> { new MessageTemplate { MessageKey = ErrorMessages.VariantNotFound } }
                };
            }

            // Get the default packing unit for the variant
            var defaultPackingUnit = await _variantPackingUnitRepository.GetQuery()
                .AsNoTracking()
                .Where(e => e.VariantId == variantId && e.IsDefaultPackingUnit)
                .FirstOrDefaultAsync();
            if (defaultPackingUnit == null)
            {
                return new ApiResponse<bool>
                {
                    IsSuccess = false,
                    StatusCode = HttpStatusCode.BadRequest,
                    Errors = new List<MessageTemplate> { new MessageTemplate { MessageKey = ErrorMessages.DefaultPackingUnitNotFound } }
                };
            }

            // Use the default packing unit for stock balance, not the transaction packing unit
            var defaultPackingUnitId = defaultPackingUnit.PackingUnitId;

            // Get existing stock balance
            var existingStockBalance = await _repository.GetByVariantPackingUnitAndBranchWithoutInclues(variantId, defaultPackingUnitId, branchId);

            if (existingStockBalance == null)
            {
                // Create new stock balance record
                var newStockBalance = new StockBalance
                {
                    VariantId = variantId,
                    PackingUnitId = defaultPackingUnitId, // Use default packing unit
                    BranchId = branchId,
                    CurrentBalance = isReceipt ? quantity : -quantity,
                    MinimumBalance = 0,
                    MaximumBalance = 0,
                    UnitCost = unitCost,
                    TotalCost = (isReceipt ? quantity : -quantity) * unitCost
                };

                await _repository.Add(newStockBalance);
            }
            else
            {
                // Update existing stock balance
                var newBalance = existingStockBalance.CurrentBalance + (isReceipt ? quantity : -quantity);
                var newTotalCost = existingStockBalance.TotalCost + (isReceipt ? quantity * unitCost : -quantity * unitCost);

                // Calculate new average unit cost
                var newUnitCost = newBalance > 0 ? newTotalCost / newBalance : unitCost;

                existingStockBalance.CurrentBalance = newBalance;
                existingStockBalance.TotalCost = newTotalCost;
                existingStockBalance.UnitCost = newUnitCost;

                await _repository.Update(existingStockBalance);
            }

            return new ApiResponse<bool>
            {
                IsSuccess = true,
                StatusCode = HttpStatusCode.OK,
                Result = true
            };
        }
        catch (Exception ex)
        {
            return new ApiResponse<bool>
            {
                IsSuccess = false,
                StatusCode = HttpStatusCode.BadRequest,
                Errors = new List<MessageTemplate> { new MessageTemplate { MessageKey = ex.Message } }
            };
        }
    }
}