using Inventory.Domain.Models.Entities.ManufacturerCompanies;

namespace Inventory.Infrastructure.Data.DbConfig.ManufacturerCompanies;

public class ManufacturerCompanyDbConfig : BaseSettingEntityDbConfig<ManufacturerCompany>
{
    protected override EntityTypeBuilder<ManufacturerCompany> ApplyConfiguration(EntityTypeBuilder<ManufacturerCompany> builder)
    {
        base.ApplyConfiguration(builder);
        builder.ToTable("ManufacturerCompanies");
        return builder;
    }
}
