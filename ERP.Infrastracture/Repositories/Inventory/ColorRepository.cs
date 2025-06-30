using ERP.Application.Repositories.Inventory;
using ERP.Domain.Models.Entities.Inventory.Colors;
using ERP.Infrastracture.Repositories.BaseRepositories;

namespace ERP.Infrastracture.Repositories.Inventory;

public class ColorRepository : BaseSettingRepository<Color>, IColorRepository
{
    public ColorRepository(IApplicationDbContext context) : base(context) { }
}