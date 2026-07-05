using ERP.BuildingBlocks.Application.Repositories.BaseRepositories;
using Accounting.Domain.Models.Entities.AccountGuides;

namespace Accounting.Application.Repositories;

/// <summary>
/// Repository contract for account guide persistence.
/// </summary>
public interface IAccountGuideRepository : IBaseSettingRepository<AccountGuide>;
