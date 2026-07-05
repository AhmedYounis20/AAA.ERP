using ERP.BuildingBlocks.Application.Dtos.BaseDtos;

namespace Accounting.Domain.OutputDtos;

public class EntryNumberDto
{
    public string EntryNumber { get; set; }
    public Guid FinancialPeriodId { get; set; }
    public string FinancialPeriodNumber { get; set; }
}