using AAA.ERP.OutputDtos.BaseDtos;
using Domain.Account.Models.Dtos.Attachments;
using Domain.Account.Models.Dtos.Entry;
using Domain.Account.Models.Entities.Currencies;
using Domain.Account.Models.Entities.Entries;
using Domain.Account.Models.Entities.FinancialPeriods;
using Domain.Account.Models.Entities.SubLeadgers;

namespace AAA.ERP.OutputDtos;

public class EntryDto 
{
    public Guid Id { get; set; }
    public Guid? CreatedBy { get; set; } 
    public Guid? ModifiedBy { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime ModifiedAt { get; set; }
    public string EntryNumber { get; set; } = string.Empty;
    public string? DocumentNumber { get; set; }
    public Guid? CurrencyId { get; set; }
    public virtual Currency? Currency { get; set; }
    public decimal ExchageRate { get; set; }
    public string? ReceiverName { get; set; }
    public Guid BranchId { get; set; }
    public Branch? Branch { get; set; }
    public DateTime EntryDate { get; set; }
    public string? Notes { get; set; }
    public Guid FinancialPeriodId { get; set; }
    public string? FinancialPeriodNumber { get; set; }
    public virtual List<AttachmentDto> Attachments { get; set; }
    public virtual List<ComplexFinancialTransactionDto>  FinancialTransactions { get; set; }
}