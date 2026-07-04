using Accounting.Application.Commands.AccountGuides;
using ERP.Application.Validators.Account.ComandValidators.BaseCommandValidators.CreateCommandValidators;
using ERP.Domain.Models.Entities.Account.AccountGuides;

namespace Accounting.Application.Validators.AccountGuides;

public class AccountGuideCreateValidator : BaseSettingCreateValidator<AccountGuideCreateCommand, AccountGuide>;
