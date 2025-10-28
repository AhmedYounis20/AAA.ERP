using Account.Domain.Commands.SubLeadgers.CashInBoxes;
using Account.Domain.Models.Entities.SubLeadgers;
using Shared.Application.Validators.BaseCommandValidators.CreateCommandValidators;

namespace Account.Application.Validators.CommandValidators.SubLeadgers.CashInBoxes;

public class CashInBoxCreateValidator : BaseSubLeadgerCreateValidator<CashInBoxCreateCommand, CashInBox>
{
    public CashInBoxCreateValidator() : base()
    { }
}