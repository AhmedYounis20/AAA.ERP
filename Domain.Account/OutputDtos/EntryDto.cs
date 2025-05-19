using Domain.Account.Models.Dtos.Attachments;
using ERP.Domain.Models.Entities.Account.Currencies;
using ERP.Domain.Models.Entities.Account.Entries;
using ERP.Domain.Models.Entities.Account.SubLeadgers;

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
    public decimal ExchangeRate { get; set; }
    public string? ReceiverName { get; set; }
    public Guid BranchId { get; set; }
    public Branch? Branch { get; set; }
    public DateTime EntryDate { get; set; }
    public string? Notes { get; set; }
    public Guid FinancialPeriodId { get; set; }
    public string? FinancialPeriodNumber { get; set; }
    public virtual List<AttachmentDto> Attachments { get; set; }
    public virtual List<FinancialTransaction>  FinancialTransactions { get; set; }
}