using Accounting.Application.Commands.CashInBoxes;
using ERP.Application.Validators.Account.ComandValidators.SubLeadgers.BaseSubLeadgers;
using ERP.Domain.Models.Entities.Account.SubLeadgers;

namespace Accounting.Application.Validators.CashInBoxes;

public class CashInBoxCreateValidator : BaseSubLeadgerCreateValidator<CashInBoxCreateCommand, CashInBox>
{
    public CashInBoxCreateValidator() : base()
    {
    }
}
