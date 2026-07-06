using ERP.BuildingBlocks.Persistence.Configurations;
using Inventory.Domain.Models.Entities.ManufacturerCompanies;
using ERP.BuildingBlocks.Persistence;

namespace Inventory.Infrastructure.Persistence;

public class ManufacturerCompanyDbConfig : BaseSettingEntityDbConfig<ManufacturerCompany>
{
    protected override EntityTypeBuilder<ManufacturerCompany> ApplyConfiguration(EntityTypeBuilder<ManufacturerCompany> builder)
    {
        base.ApplyConfiguration(builder);
        builder.ToTable("ManufacturerCompanies", DbSchemaNames.Inventory);
        return builder;
    }
}
