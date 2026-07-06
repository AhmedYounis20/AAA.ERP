using Accounting.Application.Commands.AccountGuides;
using Accounting.Application.Repositories;
using Accounting.Domain.Models.Entities.AccountGuides;
using ERP.BuildingBlocks.Application.CQRS.Handlers.Setting;

namespace Accounting.Infrastructure.Handlers.AccountGuides;

public sealed class AccountGuideUpdateCommandHandler(IAccountGuideRepository repository)
    : BaseSettingUpdateHandler<AccountGuide, AccountGuideUpdateCommand>(repository);
