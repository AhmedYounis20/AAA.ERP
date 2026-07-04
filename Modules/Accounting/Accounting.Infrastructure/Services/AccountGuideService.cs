using Accounting.Application.Commands.AccountGuides;
using Accounting.Application.Repositories;
using Accounting.Application.Services;
using ERP.Domain.Models.Entities.Account.AccountGuides;
using ERP.Infrastracture.Services.BaseServices;

namespace Accounting.Infrastructure.Services;

public class AccountGuideService
    : BaseSettingService<AccountGuide, AccountGuideCreateCommand, AccountGuideUpdateCommand>, IAccountGuideService
{
    public AccountGuideService(IAccountGuideRepository repository) : base(repository) { }
}
