using Account.Domain.Commands.AccountGuides;
using Account.Domain.Models.Entities.AccountGuides;
using Shared.Application.BaseServices;

namespace Account.Application.Services;

public interface IAccountGuideService : IBaseSettingService<AccountGuide, AccountGuideCreateCommand, AccountGuideUpdateCommand> { }