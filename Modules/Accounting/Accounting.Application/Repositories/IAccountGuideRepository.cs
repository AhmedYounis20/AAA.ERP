using ERP.BuildingBlocks.Application.Repositories.BaseRepositories;
using Accounting.Domain.Models.Entities.AccountGuides;

namespace Accounting.Application.Repositories;

public interface IAccountGuideRepository : IBaseSettingRepository<AccountGuide>;
