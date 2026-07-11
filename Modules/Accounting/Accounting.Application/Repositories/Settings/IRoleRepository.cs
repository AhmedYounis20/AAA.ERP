using ERP.BuildingBlocks.Application.Repositories.BaseRepositories;
using Accounting.Domain.Models.Entities.Roles;

namespace Accounting.Application.Repositories;

/// <summary>
/// Repository contract for role persistence.
/// </summary>
public interface IRoleRepository : IBaseSettingRepository<Role>;
