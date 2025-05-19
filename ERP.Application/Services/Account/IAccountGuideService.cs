using ERP.Application.Services.BaseServices;
using ERP.Domain.Commands.Account.AccountGuides;
using ERP.Domain.Models.Entities.Account.AccountGuides;

namespace ERP.Application.Services.Account;

public interface IAccountGuideService : IBaseSettingService<AccountGuide, AccountGuideCreateCommand, AccountGuideUpdateCommand> { }