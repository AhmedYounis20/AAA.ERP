using Domain.Account.DBConfiguration.Config.BaseConfig;
using ERP.Domain.Models.Entities.Inventory.ManufacturerCompanies;

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
