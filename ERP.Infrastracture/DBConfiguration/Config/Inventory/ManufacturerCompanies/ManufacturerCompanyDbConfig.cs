using Domain.Account.DBConfiguration.Config.BaseConfig;
using ERP.Domain.Models.Entities.Inventory.ManufacturerCompanies;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ERP.Infrastracture.DBConfiguration.Config.Inventory.ManufacturerCompanies
{
    public class ManufacturerCompanyDbConfig : BaseSettingEntityDbConfig<ManufacturerCompany>
    {
        protected override EntityTypeBuilder<ManufacturerCompany> ApplyConfiguration(EntityTypeBuilder<ManufacturerCompany> builder)
        {
            base.ApplyConfiguration(builder);
            builder.ToTable("ManufacturerCompanies");
            return builder;
        }
    }
}