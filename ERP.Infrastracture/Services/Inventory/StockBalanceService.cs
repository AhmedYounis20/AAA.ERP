using ERP.Application.Repositories.Inventory;
using ERP.Application.Services.Inventory;
using ERP.Domain.Commands.Inventory.StockBalances;
using ERP.Domain.Models.Entities.Inventory.Sizes;
using ERP.Infrastracture.Services.BaseServices;
using Shared.Responses;

namespace ERP.Infrastracture.Services.Inventory;

public class StockBalanceService : BaseService<StockBalance, StockBalanceCreateCommand, StockBalanceUpdateCommand>, IStockBalanceService
{
    private readonly IStockBalanceRepository _repository;
    private readonly IItemRepository _itemRepository;
    private readonly IUnitOfWork _unitOfWork;

    public StockBalanceService(
        IStockBalanceRepository repository,
        IItemRepository itemRepository, IUnitOfWork unitOfWork) : base(repository)
    {
        _repository = repository;
        _itemRepository = itemRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<ApiResponse<StockBalance>> GetByItemAndBranch(Guid itemId, Guid branchId)
    {
        try
        {
            var stockBalance = await _repository.GetByItemAndBranch(itemId, branchId);
            return new ApiResponse<StockBalance>
            {
                IsSuccess = true,
                StatusCode = HttpStatusCode.OK,
                Result = stockBalance
            };
        }
        catch (Exception ex)
        {
            return new ApiResponse<StockBalance>
            {
                IsSuccess = false,
                StatusCode = HttpStatusCode.BadRequest,
                ErrorMessages = [ex.Message]
            };
        }
    }

    public async Task<ApiResponse<StockBalance>> GetByItemPackingUnitAndBranch(Guid itemId, Guid packingUnitId, Guid branchId)
    {
        try
        {
            var stockBalance = await _repository.GetByItemPackingUnitAndBranch(itemId, packingUnitId, branchId);
            return new ApiResponse<StockBalance>
            {
                IsSuccess = true,
                StatusCode = HttpStatusCode.OK,
                Result = stockBalance
            };
        }
        catch (Exception ex)
        {
            return new ApiResponse<StockBalance>
            {
                IsSuccess = false,
                StatusCode = HttpStatusCode.BadRequest,
                ErrorMessages = [ex.Message]
            };
        }
    }

    public async Task<ApiResponse<IEnumerable<StockBalance>>> GetByBranch(Guid branchId)
    {
        try
        {
            var stockBalances = await _repository.GetByBranch(branchId);
            return new ApiResponse<IEnumerable<StockBalance>>
            {
                IsSuccess = true,
                StatusCode = HttpStatusCode.OK,
                Result = stockBalances
            };
        }
        catch (Exception ex)
        {
            return new ApiResponse<IEnumerable<StockBalance>>
            {
                IsSuccess = false,
                StatusCode = HttpStatusCode.BadRequest,
                ErrorMessages = [ex.Message]
            };
        }
    }

    public async Task<ApiResponse<IEnumerable<StockBalance>>> GetByItem(Guid itemId)
    {
        try
        {
            var stockBalances = await _repository.GetByItem(itemId);
            return new ApiResponse<IEnumerable<StockBalance>>
            {
                IsSuccess = true,
                StatusCode = HttpStatusCode.OK,
                Result = stockBalances
            };
        }
        catch (Exception ex)
        {
            return new ApiResponse<IEnumerable<StockBalance>>
            {
                IsSuccess = false,
                StatusCode = HttpStatusCode.BadRequest,
                ErrorMessages = [ex.Message]
            };
        }
    }

    public async Task<ApiResponse<decimal>> GetCurrentBalance(Guid itemId, Guid packingUnitId, Guid branchId)
    {
        try
        {
            var currentBalance = await _repository.GetCurrentBalance(itemId, packingUnitId, branchId);
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
                ErrorMessages = [ex.Message]
            };
        }
    }

    public async Task<ApiResponse<bool>> UpdateStockBalance(Guid itemId, Guid packingUnitId, Guid branchId, decimal quantity, decimal unitCost, bool isReceipt)
    {
        try
        {
            // Get the item to find its default packing unit
            var item = await _itemRepository.Get(itemId);
            if (item == null)
            {
                return new ApiResponse<bool>
                {
                    IsSuccess = false,
                    StatusCode = HttpStatusCode.BadRequest,
                    ErrorMessages = ["ItemNotFound"]
                };
            }

            // Get the default packing unit for the item
            var defaultPackingUnit = await _unitOfWork.ItemPackingUnitRepository.GetQuery().AsNoTracking().Where(e => e.ItemId == itemId && e.IsDefaultPackingUnit).FirstOrDefaultAsync();
            if(defaultPackingUnit == null)
            {
                return new ApiResponse<bool>
                {
                    IsSuccess = false,
                    StatusCode = HttpStatusCode.BadRequest,
                    ErrorMessages = ["DefaultPackingUnitNotFound"]
                };
            }

            // Use the default packing unit for stock balance, not the transaction packing unit
            var defaultPackingUnitId = defaultPackingUnit.PackingUnitId;

            // Get existing stock balance
            var existingStockBalance = await _repository.GetByItemPackingUnitAndBranchWithoutInclues(itemId, defaultPackingUnitId, branchId);

            if (existingStockBalance == null)
            {
                // Create new stock balance record
                var newStockBalance = new StockBalance
                {
                    ItemId = itemId,
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
                ErrorMessages = [ex.Message]
            };
        }
    }
} 