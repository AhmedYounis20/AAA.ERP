using ERP.Application.Validators.Account.ComandValidators.SubLeadgers.BaseSubLeadgers;
using ERP.Domain.Commands.Account.SubLeadgers.CashInBoxes;
using ERP.Domain.Models.Entities.Account.SubLeadgers;

namespace ERP.Application.Validators.Account.ComandValidators.SubLeadgers.CashInBoxes;

public class CashInBoxCreateValidator : BaseSubLeadgerCreateValidator<CashInBoxCreateCommand, CashInBox>
{
    public CashInBoxCreateValidator() : base()
    { }
}