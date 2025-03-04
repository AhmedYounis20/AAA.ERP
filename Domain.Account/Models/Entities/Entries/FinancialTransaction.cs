using Domain.Account.Models.Entities.ChartOfAccounts;
using Domain.Account.Models.Entities.CollectionBooks;
using Domain.Account.Models.Entities.Currencies;
using Domain.Account.Models.Entities.FinancialPeriods;
using Domain.Account.Models.Entities.SubLeadgers;
using Shared.BaseEntities;

namespace Domain.Account.Models.Entities.Entries;

public class FinancialTransaction : BaseEntity
{
    public Guid EntryId { get; set; }
    public virtual Entry? Entry { get; set; }
    public decimal Amount { get; set; }
    public Guid ChartOfAccountId { get; set; }
    public ChartOfAccount? ChartOfAccount { get; set; }
    public Guid? ComplementTransactionId { get; set; }
    public string? Notes { get; set; }
    public AccountNature AccountNature { get; set; }
    public PaymentType PaymentType { get; set; }
    public Guid? ChequeBankId { get; set; }
    public Bank? ChequeBank { get; set; }
    public string? ChequeNumber { get; set; }
    public string? ChequeIssueDate { get; set; }
    public DateTime? ChequeCollectionDate { get; set; }

    public Guid? CollectionBookId { get; set; }
    public CollectionBook? CollectionBook { get; set; }
    public string? CashAgentName { get; set; }
    public string? CashPhoneNumber { get; set; }
    
    public string? PromissoryName { get; set; }
    public string? PromissoryNumber { get; set; }
    public string? PromissoryIdentityCard { get; set; }
    public DateTime? PromissoryCollectionDate { get; set; }
    
    public string? WireTransferReferenceNumber { get; set; }
    public string? AtmReferenceNumber { get; set; }
    
    public string? CreditCardLastDigits { get; set; }
    public bool IsPaymentTransaction { get; set; } = true;
    public int OrderNumber { get; set; }
}       