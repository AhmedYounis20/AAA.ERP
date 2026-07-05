using Accounting.Application.Commands.AccountGuides;
using ERP.BuildingBlocks.Application.Services.BaseServices;
using Accounting.Domain.Models.Entities.AccountGuides;

namespace Accounting.Application.Services;

/// <summary>
/// Application service contract for account guide.
/// </summary>
public interface IAccountGuideService : IBaseSettingService<AccountGuide, AccountGuideCreateCommand, AccountGuideUpdateCommand>;
