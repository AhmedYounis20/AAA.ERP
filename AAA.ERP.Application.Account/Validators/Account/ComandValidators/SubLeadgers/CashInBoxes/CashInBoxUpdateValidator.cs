using Domain.Account.Commands.SubLeadgers.CashInBoxes;
using Domain.Account.Models.Entities.SubLeadgers;
using ERP.Application.Validators.Account.ComandValidators.SubLeadgers.BaseSubLeadgers;

namespace ERP.Application.Validators.Account.ComandValidators.SubLeadgers.CashInBoxes;
public class CashInBoxUpdateValidator : BaseSubLeadgerUpdateValidator<CashInBoxUpdateCommand, CashInBox>
{
    public CashInBoxUpdateValidator() : base()
    { }
}