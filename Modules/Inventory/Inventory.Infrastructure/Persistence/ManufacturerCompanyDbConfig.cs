using Domain.Account.DBConfiguration.Config.BaseConfig;
using Inventory.Domain.Models.Entities.ManufacturerCompanies;

namespace Inventory.Infrastructure.Persistence;

public class ManufacturerCompanyDbConfig : BaseSettingEntityDbConfig<ManufacturerCompany>
{
    protected override EntityTypeBuilder<ManufacturerCompany> ApplyConfiguration(EntityTypeBuilder<ManufacturerCompany> builder)
    {
        base.ApplyConfiguration(builder);
        builder.ToTable("ManufacturerCompanies");
        return builder;
    }
}
