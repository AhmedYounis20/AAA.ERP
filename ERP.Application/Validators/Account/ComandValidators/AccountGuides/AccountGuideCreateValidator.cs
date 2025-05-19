using ERP.Application.Validators.Account.ComandValidators.BaseCommandValidators.CreateCommandValidators;
using ERP.Domain.Commands.Account.AccountGuides;
using ERP.Domain.Models.Entities.Account.AccountGuides;
using Shared.Responses;

namespace ERP.Application.Validators.Account.ComandValidators.AccountGuides;

public class AccountGuideCreateValidator : BaseSettingCreateValidator<AccountGuideCreateCommand, AccountGuide>
{ }