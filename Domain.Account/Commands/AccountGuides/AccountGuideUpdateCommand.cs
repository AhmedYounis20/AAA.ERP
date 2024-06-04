using Domain.Account.Commands.BaseInputModels.BaseUpdateCommands;
using Domain.Account.Models.Entities.AccountGuide;
using Shared.Responses;

namespace Domain.Account.Commands.AccountGuides;

public class AccountGuideUpdateCommand : BaseSettingUpdateCommand<ApiResponse<AccountGuide>>
{ }