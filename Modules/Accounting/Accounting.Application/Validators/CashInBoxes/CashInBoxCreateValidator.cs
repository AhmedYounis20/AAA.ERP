using Accounting.Application.Commands.CashInBoxes;
using Accounting.Application.Validators.Account.ComandValidators.SubLeadgers.BaseSubLeadgers;
using Accounting.Domain.Models.Entities.SubLeadgers;

namespace Accounting.Application.Validators.CashInBoxes;

public class CashInBoxCreateValidator : BaseSubLeadgerCreateValidator<CashInBoxCreateCommand, CashInBox>
{
    public CashInBoxCreateValidator() : base()
    {
    }
}
