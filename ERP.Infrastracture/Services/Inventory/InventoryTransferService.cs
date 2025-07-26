using ERP.Application.Repositories.Inventory;
using ERP.Application.Services.Inventory;
using ERP.Domain.Models.Entities.Inventory;
using ERP.Domain.Commands.Inventory;
using MediatR;
using ERP.Domain.Models.Entities.Account.FinancialPeriods;
using ERP.Domain.Models.Entities.Account.Currencies;
using ERP.Domain.Models.Entities.Account.SubLeadgers;
using ERP.Domain.Models.Entities.Account.ChartOfAccounts;
using ERP.Domain.Commands.Account.Entries.CompinedEntries;
using Domain.Account.Models.Dtos.Entry;
using ERP.Domain.Models.Entities.Inventory.Sizes;
using ERP.Domain.Models.Entities.Inventory.Items;

namespace ERP.Infrastracture.Services.Inventory;

public class InventoryTransferService : BaseService<InventoryTransfer, InventoryTransferCreateCommand, InventoryTransferUpdateCommand>, IInventoryTransferService
{
    private readonly IInventoryTransferRepository _repository;
    private readonly IMediator _mediator;
    private readonly IUnitOfWork _unitOfWork;
    private readonly ApplicationDbContext _dbContext;
    public InventoryTransferService(IInventoryTransferRepository repository, IUnitOfWork unitOfWork, ApplicationDbContext dbContext, IMediator mediator) : base(repository)
    {
        _repository = repository;
        _unitOfWork = unitOfWork;
        _dbContext = dbContext;
        _mediator = mediator;
    }

    public override async Task<ApiResponse<InventoryTransfer>> Create(InventoryTransferCreateCommand command, bool isValidate = true)
    {
        var transfer = new InventoryTransfer
        {
            SourceBranchId = command.SourceBranchId,
            DestinationBranchId = command.DestinationBranchId,
            Notes = command.Notes,
            TransferType = command.TransferType,
            Status = command.TransferType == InventoryTransferType.Direct ? InventoryTransferStatus.Approved : InventoryTransferStatus.Pending,
            ApprovedAt = command.TransferType == InventoryTransferType.Direct ? DateTime.Now : null,
            Items = command.Items.Select(i => new InventoryTransferItem
            {
                ItemId = i.ItemId,
                PackingUnitId = i.PackingUnitId,
                Quantity = i.Quantity,
            }).ToList()
        };
        await _unitOfWork.BeginTransactionAsync();
        try
        {
            await _repository.Add(transfer);
            var sourceBranch = await _dbContext.Set<Branch>().AsNoTracking().FirstOrDefaultAsync(b => b.Id == transfer.SourceBranchId);
            var destBranch = await _dbContext.Set<Branch>().AsNoTracking().FirstOrDefaultAsync(b => b.Id == transfer.DestinationBranchId);
            if (sourceBranch == null || destBranch == null || sourceBranch.ChartOfAccountId == null || destBranch.ChartOfAccountId == null)
            {
                await _unitOfWork.RollbackAsync();
                return new ApiResponse<InventoryTransfer> { IsSuccess = false, ErrorMessages = ["BranchOrBranchAccountNotFound"] };
            }
            var sourceAccountId = sourceBranch.ChartOfAccountId.Value;
            var destAccountId = command.TransferType == InventoryTransferType.Direct ?  destBranch.ChartOfAccountId.Value : Guid.Parse(SD.StockUnderTransfer_Branch);
            var distinationBranchId =command.TransferType == InventoryTransferType.Direct ? command.DestinationBranchId : destAccountId;
            var moveResult = await MoveItems(command.SourceBranchId, distinationBranchId, transfer.Items);
            if (!moveResult.IsSuccess)
            {
                await _unitOfWork.RollbackAsync();
                return new ApiResponse<InventoryTransfer>
                {
                    IsSuccess = moveResult.IsSuccess,
                    ErrorMessages = moveResult.ErrorMessages,
                    StatusCode = HttpStatusCode.BadRequest
                };
            }
            
            (bool flowControl, ApiResponse<InventoryTransfer> value, CompinedEntryCreateCommand? compinedEntryCommand) = await CreateEntryCommand(transfer.Items,sourceBranch.Id, sourceAccountId, destAccountId);
            if (!flowControl || compinedEntryCommand == null)
            {
                return value;
            }
            var entryResult = await _mediator.Send(compinedEntryCommand);
            if (!entryResult.IsSuccess)
            {
                await _unitOfWork.RollbackAsync();
                return new ApiResponse<InventoryTransfer> { IsSuccess = false, ErrorMessages = entryResult.ErrorMessages };
            }
            await _unitOfWork.CommitAsync();
            return new ApiResponse<InventoryTransfer> { IsSuccess = true, Result = transfer };
        }
        catch (Exception ex)
        {
            await _unitOfWork.RollbackAsync();
            return new ApiResponse<InventoryTransfer> { IsSuccess = false, Result = transfer,ErrorMessages=["Operation Faild"] };
        }
    }


    private async Task<ApiResponse> MoveItems(Guid SourceId, Guid DistinationId, List<InventoryTransferItem> items)
    {
        foreach (var item in items)
        {
            var itemUsedPackingUnit = await _dbContext.Set<ItemPackingUnit>().AsNoTracking().FirstOrDefaultAsync(e => e.ItemId == item.ItemId && item.PackingUnitId == e.PackingUnitId);
            if (itemUsedPackingUnit == null)
            {
                return new ApiResponse
                {
                    IsSuccess = false,
                    ErrorMessages = ["FaildToGetUsedPackingUnit"],
                    StatusCode = HttpStatusCode.BadRequest
                };
            }
            decimal quantity = item.Quantity / itemUsedPackingUnit.PartsCount;

            var sourceStock = await _dbContext.Set<StockBalance>().AsNoTracking().FirstOrDefaultAsync(e=>e.ItemId == item.ItemId && e.BranchId == SourceId);
            if (sourceStock == null || sourceStock.CurrentBalance < quantity)
            {
                return new ApiResponse
                {
                    IsSuccess = false,
                    ErrorMessages = ["ItemWithNowStockBalance"],
                    StatusCode = HttpStatusCode.BadRequest
                };
            }

            sourceStock.CurrentBalance -= quantity;
            sourceStock.TotalCost -= (quantity * sourceStock.UnitCost);
            sourceStock.ModifiedAt = DateTime.Now;
            await _unitOfWork.StockBalanceRepository.Update(sourceStock);

            var distinationStock = await _dbContext.Set<StockBalance>().AsNoTracking().FirstOrDefaultAsync(e=>e.ItemId == item.ItemId && e.BranchId == DistinationId);
            if (distinationStock == null)
            {
                var distinationBranch = await _dbContext.Set<Branch>().AsNoTracking().FirstOrDefaultAsync(e => e.Id == DistinationId);
                if (distinationBranch == null)
                {
                    return new ApiResponse
                    {
                        IsSuccess = false,
                        ErrorMessages = ["DistinationBranchNotFound"],
                        StatusCode = HttpStatusCode.BadRequest
                    };
                }
                var defaultPackingUnit = await _dbContext.Set<ItemPackingUnit>().AsNoTracking().FirstOrDefaultAsync(e => e.IsDefaultPackingUnit && e.ItemId == item.ItemId);
                var usedPackingUnit = await _dbContext.Set<ItemPackingUnit>().AsNoTracking().FirstOrDefaultAsync(e => e.PackingUnitId == item.PackingUnitId && e.ItemId == item.ItemId);
                distinationStock = new StockBalance
                {
                    ItemId = item.ItemId,
                    BranchId = DistinationId,
                    CreatedAt = DateTime.Now,
                    PackingUnitId = defaultPackingUnit?.PackingUnitId ?? Guid.Empty,
                    TotalCost = (usedPackingUnit?.AverageCostPrice ?? 0) * item.Quantity,
                    UnitCost = usedPackingUnit?.AverageCostPrice ?? 0,
                    ModifiedAt = DateTime.Now,
                    CurrentBalance =quantity
                };
                await _unitOfWork.StockBalanceRepository.Add(distinationStock);
            }
            else
            {
                var defaultPackingUnit = await _dbContext.Set<ItemPackingUnit>().AsNoTracking().FirstOrDefaultAsync(e => e.IsDefaultPackingUnit && e.ItemId == item.ItemId);

                distinationStock.CurrentBalance += quantity;
                distinationStock.ModifiedAt = DateTime.Now;
                distinationStock.UnitCost = (distinationStock.TotalCost + ((defaultPackingUnit?.AverageCostPrice ?? 0) * quantity)) / (distinationStock.CurrentBalance);
                distinationStock.TotalCost = distinationStock.UnitCost * distinationStock.CurrentBalance;
                await _unitOfWork.StockBalanceRepository.Update(distinationStock);
            }

        }

        return new ApiResponse
        {
            IsSuccess = true,
            StatusCode = HttpStatusCode.OK
        };
    }
    private async Task<(bool flowControl, ApiResponse<InventoryTransfer> result, CompinedEntryCreateCommand? command)> CreateEntryCommand(List<InventoryTransferItem> items,Guid BranchId, Guid sourceAccountId, Guid destAccountId)
    {
        var defaultCurrency = await _dbContext.Set<Currency>().AsNoTracking().FirstOrDefaultAsync(e => e.IsDefault);
        if (defaultCurrency == null)
        {
            await _unitOfWork.RollbackAsync();
            return (flowControl: false, value: new ApiResponse<InventoryTransfer> { IsSuccess = false, ErrorMessages = ["DefaultCurrencyNotFound"] }, null);
        }
        // Get financial period for today
        var now = DateTime.UtcNow;
        var financialPeriod = await _dbContext.Set<FinancialPeriod>().AsNoTracking().FirstOrDefaultAsync(e => e.StartDate <= now && e.EndDate > now);
        if (financialPeriod == null)
        {
            await _unitOfWork.RollbackAsync();
            return (flowControl: false, value: new ApiResponse<InventoryTransfer> { IsSuccess = false, ErrorMessages = ["NotFoundCurrentFinancialPeriod"] }, command: null);
        }
        // Get source and destination branch accounts

        decimal totalPrice = 0;
        foreach (var item in items)
        {
            var itemUsedpackingUnit = await _dbContext.Set<ItemPackingUnit>().AsNoTracking().FirstOrDefaultAsync(e => e.PackingUnitId == item.PackingUnitId && e.ItemId == item.ItemId);

            totalPrice += (item.Quantity * (itemUsedpackingUnit?.AverageCostPrice??0));
        }

        // 
        // Calculate total amount (sum of all item quantities, or use cost if available)
        var compinedEntryCommand = new CompinedEntryCreateCommand
        {
            BranchId = BranchId,
            CurrencyId = defaultCurrency.Id,
            ExchangeRate = defaultCurrency.ExchangeRate,
            EntryDate = now,
            FinancialPeriodId = financialPeriod.Id,
            FinancialTransactions = new List<ComplexFinancialTransactionDto>
                {
                    new ComplexFinancialTransactionDto
                    {
                        DebitAccountId = destAccountId,
                        CreditAccountId = sourceAccountId,
                        Amount = totalPrice,
                        PaymentType = PaymentType.None
                    }
                }
        };
        return (flowControl: true, value: null, compinedEntryCommand);
    }

    public override async Task<ApiResponse<InventoryTransfer>> Update(InventoryTransferUpdateCommand command, bool isValidate = true)
    {
        var transfer = await _repository.GetWithDetails(command.Id);
        if (transfer == null)
            return new ApiResponse<InventoryTransfer> { IsSuccess = false, ErrorMessages = ["Transfer not found"] };
        transfer.SourceBranchId = command.SourceBranchId;
        transfer.DestinationBranchId = command.DestinationBranchId;
        transfer.Notes = command.Notes;
        // Update items (simple replace for brevity)
        transfer.Items.Clear();
        foreach (var i in command.Items)
        {
            transfer.Items.Add(new InventoryTransferItem
            {
                ItemId = i.ItemId,
                PackingUnitId = i.PackingUnitId,
                Quantity = i.Quantity,
            });
        }
        _repository.Update(transfer);
        return new ApiResponse<InventoryTransfer> { IsSuccess = true, Result = transfer };
    }

    public async Task<ApiResponse<InventoryTransfer>> GetWithDetails(Guid id)
    {
        var transfer = await _repository.GetWithDetails(id);
        if (transfer == null)
            return new ApiResponse<InventoryTransfer> { IsSuccess = false, ErrorMessages = ["Transfer not found"] };
        return new ApiResponse<InventoryTransfer> { IsSuccess = true, Result = transfer };
    }

    public async Task<ApiResponse<IEnumerable<InventoryTransfer>>> GetByStatus(InventoryTransferStatus status)
    {
        var transfers = await _repository.GetByStatus(status);
        return new ApiResponse<IEnumerable<InventoryTransfer>> { IsSuccess = true, Result = transfers };
    }

    public async Task<ApiResponse<InventoryTransfer>> ApproveTransfer(Guid id)
    {
        var transfer = await _repository.Get(id);
        if (transfer == null)
            return new ApiResponse<InventoryTransfer> { IsSuccess = false, ErrorMessages = ["Transfer not found"] };
        if (transfer.Status != InventoryTransferStatus.Pending)
            return new ApiResponse<InventoryTransfer> { IsSuccess = false, ErrorMessages = ["Transfer is not pending"] };
        await _unitOfWork.BeginTransactionAsync();
        try
        {
            transfer.Status = InventoryTransferStatus.Approved;
            // transfer.ApprovedBy = approverId;
            transfer.ApprovedAt = DateTime.UtcNow;
            await _repository.Update(transfer);
            var sourceBranch = await _dbContext.Set<Branch>().AsNoTracking().FirstOrDefaultAsync(b => b.Id == transfer.SourceBranchId);
            var destBranch = await _dbContext.Set<Branch>().AsNoTracking().FirstOrDefaultAsync(b => b.Id == transfer.DestinationBranchId);
            if (sourceBranch == null || destBranch == null || sourceBranch.ChartOfAccountId == null || destBranch.ChartOfAccountId == null)
            {
                await _unitOfWork.RollbackAsync();
                return new ApiResponse<InventoryTransfer> { IsSuccess = false, ErrorMessages = ["BranchOrBranchAccountNotFound"] };
            }
            var sourceAccountId = Guid.Parse(SD.StockUnderTransfer_Branch);
            var destAccountId =  destBranch.ChartOfAccountId.Value;
            var sourceranchId = Guid.Parse(SD.StockUnderTransfer_Branch);
            var items = await _dbContext.Set<InventoryTransferItem>().Where(e => e.InventoryTransferId == transfer.Id).ToListAsync();
            var moveResult = await MoveItems(sourceranchId, destBranch.Id, items);
            if (!moveResult.IsSuccess)
            {
                await _unitOfWork.RollbackAsync();
                return new ApiResponse<InventoryTransfer>
                {
                    IsSuccess = moveResult.IsSuccess,
                    ErrorMessages = moveResult.ErrorMessages,
                    StatusCode = HttpStatusCode.BadRequest
                };
            }

            (bool flowControl, ApiResponse<InventoryTransfer> value, CompinedEntryCreateCommand? compinedEntryCommand) = await CreateEntryCommand(items,destBranch.Id, sourceAccountId, destAccountId);
            if (!flowControl || compinedEntryCommand == null)
            {
                return value;
            }
            var entryResult = await _mediator.Send(compinedEntryCommand);
            if (!entryResult.IsSuccess)
            {
                await _unitOfWork.RollbackAsync();
                return new ApiResponse<InventoryTransfer> { IsSuccess = false, ErrorMessages = entryResult.ErrorMessages };
            }
            await _unitOfWork.CommitAsync();
            return new ApiResponse<InventoryTransfer> { IsSuccess = true, Result = transfer };          
         }
        catch (Exception ex)
        {
            await _unitOfWork.RollbackAsync();
            Console.WriteLine(ex);
            return new ApiResponse<InventoryTransfer>
            {
                IsSuccess = false,
                StatusCode = HttpStatusCode.InternalServerError
            };
        }
    }

    public async Task<ApiResponse<InventoryTransfer>> RejectTransfer(Guid id, Guid approverId, string? reason)
    {
        var transfer = await _repository.GetWithDetails(id);
        if (transfer == null)
            return new ApiResponse<InventoryTransfer> { IsSuccess = false, ErrorMessages = ["Transfer not found"] };
        if (transfer.Status != InventoryTransferStatus.Pending)
            return new ApiResponse<InventoryTransfer> { IsSuccess = false, ErrorMessages = ["Transfer is not pending"] };
               await _unitOfWork.BeginTransactionAsync();
        try
        {
            transfer.Status = InventoryTransferStatus.Rejected;
            // transfer.ApprovedBy = approverId;
            transfer.ApprovedAt = DateTime.UtcNow;
            await _repository.Update(transfer);
            var sourceBranch = await _dbContext.Set<Branch>().AsNoTracking().FirstOrDefaultAsync(b => b.Id == transfer.SourceBranchId);
            var destBranch = await _dbContext.Set<Branch>().AsNoTracking().FirstOrDefaultAsync(b => b.Id == transfer.DestinationBranchId);
            if (sourceBranch == null || destBranch == null || sourceBranch.ChartOfAccountId == null || destBranch.ChartOfAccountId == null)
            {
                await _unitOfWork.RollbackAsync();
                return new ApiResponse<InventoryTransfer> { IsSuccess = false, ErrorMessages = ["BranchOrBranchAccountNotFound"] };
            }
            var sourceAccountId = Guid.Parse(SD.StockUnderTransfer_Branch);
            var destAccountId =  sourceBranch.ChartOfAccountId.Value;
            var sourceranchId = Guid.Parse(SD.StockUnderTransfer_Branch);
            var items = await _dbContext.Set<InventoryTransferItem>().Where(e => e.InventoryTransferId == transfer.Id).ToListAsync();
            var moveResult = await MoveItems(sourceranchId, transfer.SourceBranchId, items);
            if (!moveResult.IsSuccess)
            {
                await _unitOfWork.RollbackAsync();
                return new ApiResponse<InventoryTransfer>
                {
                    IsSuccess = moveResult.IsSuccess,
                    ErrorMessages = moveResult.ErrorMessages,
                    StatusCode = HttpStatusCode.BadRequest
                };
            }

            (bool flowControl, ApiResponse<InventoryTransfer> value, CompinedEntryCreateCommand? compinedEntryCommand) = await CreateEntryCommand(items,sourceranchId, sourceAccountId, destAccountId);
            if (!flowControl || compinedEntryCommand == null)
            {
                return value;
            }
            var entryResult = await _mediator.Send(compinedEntryCommand);
            if (!entryResult.IsSuccess)
            {
                await _unitOfWork.RollbackAsync();
                return new ApiResponse<InventoryTransfer> { IsSuccess = false, ErrorMessages = entryResult.ErrorMessages };
            }
            await _unitOfWork.CommitAsync();
            return new ApiResponse<InventoryTransfer> { IsSuccess = true, Result = transfer };          
         }
        catch (Exception ex)
        {
            await _unitOfWork.RollbackAsync();
            Console.WriteLine(ex);
            return new ApiResponse<InventoryTransfer>
            {
                IsSuccess = false,
                StatusCode = HttpStatusCode.InternalServerError
            };
        }
    }
} 