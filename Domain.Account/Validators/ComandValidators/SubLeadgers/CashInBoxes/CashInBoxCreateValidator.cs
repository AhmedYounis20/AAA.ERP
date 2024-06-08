using Domain.Account.Commands.SubLeadgers.Banks;
using Domain.Account.Commands.SubLeadgers.CashInBoxes;
using Domain.Account.Models.Entities.SubLeadgers;
using Domain.Account.Validators.ComandValidators.ChartOfAccounts;

namespace Domain.Account.Validators.ComandValidators.SubLeadgers.CashInBoxes;

public class CashInBoxCreateValidator : BaseSubLeadgerCreateValidator<CashInBoxCreateCommand, CashInBox>
{
    public CashInBoxCreateValidator() : base()
    { }
}