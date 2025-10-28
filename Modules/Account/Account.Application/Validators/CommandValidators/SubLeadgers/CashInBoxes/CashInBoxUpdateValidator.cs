using Account.Domain.Commands.SubLeadgers.CashInBoxes;
using Account.Domain.Models.Entities.SubLeadgers;
using Shared.Application.Validators.BaseCommandValidators.UpdateCommandValidators;

namespace Account.Application.Validators.CommandValidators.SubLeadgers.CashInBoxes;
public class CashInBoxUpdateValidator : BaseSubLeadgerUpdateValidator<CashInBoxUpdateCommand, CashInBox>
{
    public CashInBoxUpdateValidator() : base()
    { }
}