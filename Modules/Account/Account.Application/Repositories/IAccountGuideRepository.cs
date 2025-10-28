using Account.Domain.Models.Entities.AccountGuides;
using Shared.Application.Repositories;

namespace Account.Application.Repositories;

public interface IAccountGuideRepository : IBaseSettingRepository<AccountGuide> { }