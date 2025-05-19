using Domain.Account.Commands.AccountGuides;
using Domain.Account.Models.Entities.AccountGuide;
using ERP.Application.Validators.Account.ComandValidators.BaseCommandValidators.UpdateCommandValidators;
using Shared.Responses;

namespace ERP.Application.Validators.Account.ComandValidators.AccountGuides;

public class AccountGuideUpdateValidator : BaseSettingUpdateValidator<AccountGuideUpdateCommand, AccountGuide>
{ }