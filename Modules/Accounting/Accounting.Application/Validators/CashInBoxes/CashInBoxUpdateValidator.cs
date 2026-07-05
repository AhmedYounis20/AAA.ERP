using Accounting.Application.Commands.CashInBoxes;
using ERP.Application.Validators.Account.ComandValidators.SubLeadgers.BaseSubLeadgers;
using Accounting.Domain.Models.Entities.SubLeadgers;

namespace Accounting.Application.Validators.CashInBoxes;

public class CashInBoxUpdateValidator : BaseSubLeadgerUpdateValidator<CashInBoxUpdateCommand, CashInBox>
{
    public CashInBoxUpdateValidator() : base()
    {
    }
}
