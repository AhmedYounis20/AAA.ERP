using Domain.Account.Commands.AccountGuides;
using Domain.Account.Models.Entities.AccountGuide;
using Domain.Account.Validators.ComandValidators.BaseCommandValidators.CreateCommandValidators;
using Shared.Responses;

namespace Domain.Account.Validators.ComandValidators.AccountGuides;

public class AccountGuideCreateValidator : BaseSettingCreateValidator<AccountGuideCreateCommand,AccountGuide>
{ }