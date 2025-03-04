using Domain.Account.Models.Entities.ChartOfAccounts;

namespace Domain.Account.Models.Dtos.Entry;

public class ComplexFinancialTransactionDto
{
    public Guid Id { get; set; }
    public Guid ComplementId { get; set; }
    public Guid DebitAccountId { get; set; } // chart  of account in financial with Id
    public Guid CreditAccountId { get; set; }// chart  of account in financial with ComplementId  
    public decimal Amount { get; set; }
    public string? Notes { get; set; }
    public AccountNature AccountNature { get; set; }  
    public PaymentType PaymentType { get; set; }

    public Guid? ChequeBankId { get; set; }
    public string? ChequeNumber { get; set; }
    public string? ChequeIssueDate { get; set; }
    public DateTime? ChequeCollectionDate { get; set; }

    public Guid? CollectionBookId { get; set; }
    public string? CashAgentName { get; set; }
    public string? CashPhoneNumber { get; set; }

    public string? PromissoryName { get; set; }
    public string? PromissoryNumber { get; set; }
    public string? PromissoryIdentityCard { get; set; }
    public DateTime? PromissoryCollectionDate { get; set; }
    
    public string? WireTransferReferenceNumber { get; set; }
    public string? AtmTransferReferenceNumber { get; set; }
    
    public string? CreditCardLastDigits { get; set; }
    public bool IsPaymentTransaction { get; set; } = true;

    public int OrderNumber { get; set; }
}