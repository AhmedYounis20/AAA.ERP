using Account.Domain.Models.Entities.Roles;
using Shared.Application.Repositories;

namespace Account.Application.Repositories;

public interface IRoleRepository : IBaseSettingRepository<Role> { }