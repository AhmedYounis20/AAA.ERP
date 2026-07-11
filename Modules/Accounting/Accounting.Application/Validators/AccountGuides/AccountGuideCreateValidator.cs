using Accounting.Application.Commands.AccountGuides;
using ERP.BuildingBlocks.Application.Validators.Account.CommandValidators.BaseCommandValidators.CreateCommandValidators;
using Accounting.Domain.Models.Entities.AccountGuides;

namespace Accounting.Application.Validators.AccountGuides;

public class AccountGuideCreateValidator : BaseSettingCreateValidator<AccountGuideCreateCommand, AccountGuide>;
