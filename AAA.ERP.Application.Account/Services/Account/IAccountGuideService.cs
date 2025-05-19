using Domain.Account.Commands.AccountGuides;
using Domain.Account.Models.Entities.AccountGuide;
using ERP.Application.Services.BaseServices;

namespace ERP.Application.Services.Account;

public interface IAccountGuideService : IBaseSettingService<AccountGuide, AccountGuideCreateCommand, AccountGuideUpdateCommand> { }