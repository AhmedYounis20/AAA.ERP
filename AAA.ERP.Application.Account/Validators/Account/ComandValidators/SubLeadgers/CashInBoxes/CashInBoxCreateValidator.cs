using Domain.Account.Commands.SubLeadgers.Banks;
using Domain.Account.Commands.SubLeadgers.CashInBoxes;
using Domain.Account.Models.Entities.SubLeadgers;
using ERP.Application.Validators.Account.ComandValidators.SubLeadgers.BaseSubLeadgers;

namespace ERP.Application.Validators.Account.ComandValidators.SubLeadgers.CashInBoxes;

public class CashInBoxCreateValidator : BaseSubLeadgerCreateValidator<CashInBoxCreateCommand, CashInBox>
{
    public CashInBoxCreateValidator() : base()
    { }
}