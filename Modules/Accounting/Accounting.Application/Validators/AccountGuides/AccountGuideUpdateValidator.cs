using Accounting.Application.Commands.AccountGuides;
using ERP.BuildingBlocks.Application.Validators.Account.ComandValidators.BaseCommandValidators.UpdateCommandValidators;
using Accounting.Domain.Models.Entities.AccountGuides;

namespace Accounting.Application.Validators.AccountGuides;

public class AccountGuideUpdateValidator : BaseSettingUpdateValidator<AccountGuideUpdateCommand, AccountGuide>;
