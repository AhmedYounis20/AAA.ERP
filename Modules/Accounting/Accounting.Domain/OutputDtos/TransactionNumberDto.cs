using ERP.BuildingBlocks.Application.DTOs.BaseDtos;

namespace Accounting.Domain.OutputDtos;

public class TransactionNumberDto 
{
    public string TransactionNumber { get; set; } = string.Empty;
    public Guid FinancialPeriodId { get; set; }
    public string FinancialPeriodNumber { get; set; } = string.Empty;
    public Guid BranchId { get; set; }
    public string BranchName { get; set; } = string.Empty;
} 