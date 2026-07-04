using Accounting.Application.Commands.AccountGuides;
using ERP.Application.Services.BaseServices;
using ERP.Domain.Models.Entities.Account.AccountGuides;

namespace Accounting.Application.Services;

public interface IAccountGuideService : IBaseSettingService<AccountGuide, AccountGuideCreateCommand, AccountGuideUpdateCommand>;
