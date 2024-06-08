using Domain.Account.Commands.SubLeadgers.CashInBoxes;
using Domain.Account.Models.Entities.SubLeadgers;
using Domain.Account.Validators.ComandValidators.ChartOfAccounts;

namespace Domain.Account.Validators.ComandValidators.SubLeadgers.CashInBoxes;
public class CashInBoxUpdateValidator : BaseSubLeadgerUpdateValidator<CashInBoxUpdateCommand, CashInBox>
{
    public CashInBoxUpdateValidator() : base()
    { }
}