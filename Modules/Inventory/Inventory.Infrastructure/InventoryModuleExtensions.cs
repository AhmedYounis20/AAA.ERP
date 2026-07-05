using ERP.Application.Data;
using ERP.Application.Repositories.BaseRepositories;
using ERP.Application.Repositories.Inventory;
using Inventory.Application.Repositories;
using Inventory.Application.Services;
using Inventory.Infrastructure.Persistence;
using Inventory.Infrastructure.Persistence.Products;
using Inventory.Infrastructure.Persistence.Variants;
using Inventory.Infrastructure.Repositories;
using Inventory.Infrastructure.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using ERP.Domain.Models.Entities.Inventory.Products;
using ERP.Domain.Models.Entities.Inventory.Variants;
using ERP.Infrastracture.Repositories.BaseRepositories;

namespace Inventory.Infrastructure;

public static class InventoryModuleExtensions
{
    public static IServiceCollection AddInventoryModule(this IServiceCollection services)
    {
        ModuleModelConfigurationRegistry.Register(ApplyInventoryModuleConfigurations);

        services.AddScoped<IColorRepository, ColorRepository>();
        services.AddScoped<IColorService, ColorService>();
        services.AddScoped<ISizeRepository, SizeRepository>();
        services.AddScoped<ISizeService, SizeService>();
        services.AddScoped<IPackingUnitRepository, PackingUnitRepository>();
        services.AddScoped<IPackingUnitService, PackingUnitService>();
        services.AddScoped<IManufacturerCompanyRepository, ManufacturerCompanyRepository>();
        services.AddScoped<IManufacturerCompanyService, ManufacturerCompanyService>();
        services.AddScoped<ISellingPriceRepository, SellingPriceRepository>();
        services.AddScoped<ISellingPriceService, SellingPriceService>();
        services.AddScoped<IAttributeDefinitionRepository, AttributeDefinitionRepository>();
        services.AddScoped<IAttributeDefinitionService, AttributeDefinitionService>();
        services.AddScoped<IAttributeValueRepository, AttributeValueRepository>();
        services.AddScoped<IAttributeValueService, AttributeValueService>();
        services.AddScoped<IProductRepository, ProductRepository>();
        services.AddScoped<IProductService, ProductService>();
        services.AddScoped<IBaseRepository<ProductCode>, BaseRepository<ProductCode>>();
        services.AddScoped<IBaseRepository<ProductSupplier>, BaseRepository<ProductSupplier>>();
        services.AddScoped<IBaseRepository<ProductSalesTax>, BaseRepository<ProductSalesTax>>();
        services.AddScoped<IBaseRepository<ProductPurchaseTax>, BaseRepository<ProductPurchaseTax>>();
        services.AddScoped<IBaseRepository<ProductCostCenter>, BaseRepository<ProductCostCenter>>();
        services.AddScoped<IBaseRepository<ProductManufacturerCompany>, BaseRepository<ProductManufacturerCompany>>();
        services.AddScoped<IBaseRepository<ProductAttachment>, BaseRepository<ProductAttachment>>();
        services.AddScoped<IBaseRepository<ProductPackingUnit>, BaseRepository<ProductPackingUnit>>();
        services.AddScoped<IBaseRepository<ProductPackingUnitSellingPrice>, BaseRepository<ProductPackingUnitSellingPrice>>();
        services.AddScoped<IBaseRepository<ProductSellingPriceDiscount>, BaseRepository<ProductSellingPriceDiscount>>();
        services.AddScoped<IProductAttributeDefinitionRepository, ProductAttributeDefinitionRepository>();
        services.AddScoped<IVariantRepository, VariantRepository>();
        services.AddScoped<IVariantService, VariantService>();
        services.AddScoped<IBaseRepository<VariantCode>, BaseRepository<VariantCode>>();
        services.AddScoped<IBaseRepository<VariantSupplier>, BaseRepository<VariantSupplier>>();
        services.AddScoped<IBaseRepository<VariantSalesTax>, BaseRepository<VariantSalesTax>>();
        services.AddScoped<IBaseRepository<VariantPurchaseTax>, BaseRepository<VariantPurchaseTax>>();
        services.AddScoped<IBaseRepository<VariantCostCenter>, BaseRepository<VariantCostCenter>>();
        services.AddScoped<IBaseRepository<VariantManufacturerCompany>, BaseRepository<VariantManufacturerCompany>>();
        services.AddScoped<IBaseRepository<VariantPackingUnit>, BaseRepository<VariantPackingUnit>>();
        services.AddScoped<IBaseRepository<VariantPackingUnitSellingPrice>, BaseRepository<VariantPackingUnitSellingPrice>>();
        services.AddScoped<IBaseRepository<VariantSellingPriceDiscount>, BaseRepository<VariantSellingPriceDiscount>>();
        services.AddScoped<IBaseRepository<VariantAttachment>, BaseRepository<VariantAttachment>>();
        services.AddScoped<IVariantAttributeValueRepository, VariantAttributeValueRepository>();

        return services;
    }

    public static void ApplyInventoryModuleConfigurations(this ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfiguration(new ColorDbConfig());
        modelBuilder.ApplyConfiguration(new SizeDbConfig());
        modelBuilder.ApplyConfiguration(new PackingUnitDbConfig());
        modelBuilder.ApplyConfiguration(new ManufacturerCompanyDbConfig());
        modelBuilder.ApplyConfiguration(new SellingPriceDbConfig());
        modelBuilder.ApplyConfiguration(new AttributeDefinitionDbConfig());
        modelBuilder.ApplyConfiguration(new AttributeValueDbConfig());
        modelBuilder.ApplyConfiguration(new ProductDbConfig());
        modelBuilder.ApplyConfiguration(new ProductCodeDbConfig());
        modelBuilder.ApplyConfiguration(new ProductAttachmentDbConfig());
        modelBuilder.ApplyConfiguration(new ProductSupplierDbConfig());
        modelBuilder.ApplyConfiguration(new ProductSalesTaxDbConfig());
        modelBuilder.ApplyConfiguration(new ProductPurchaseTaxDbConfig());
        modelBuilder.ApplyConfiguration(new ProductCostCenterDbConfig());
        modelBuilder.ApplyConfiguration(new ProductManufacturerCompanyDbConfig());
        modelBuilder.ApplyConfiguration(new ProductPackingUnitDbConfig());
        modelBuilder.ApplyConfiguration(new ProductPackingUnitSellingPriceDbConfig());
        modelBuilder.ApplyConfiguration(new ProductSellingPriceDiscountDbConfig());
        modelBuilder.ApplyConfiguration(new ProductAttributeDefinitionDbConfig());
        modelBuilder.ApplyConfiguration(new VariantDbConfig());
        modelBuilder.ApplyConfiguration(new VariantCodeDbConfig());
        modelBuilder.ApplyConfiguration(new VariantAttachmentDbConfig());
        modelBuilder.ApplyConfiguration(new VariantSupplierDbConfig());
        modelBuilder.ApplyConfiguration(new VariantSalesTaxDbConfig());
        modelBuilder.ApplyConfiguration(new VariantPurchaseTaxDbConfig());
        modelBuilder.ApplyConfiguration(new VariantCostCenterDbConfig());
        modelBuilder.ApplyConfiguration(new VariantManufacturerCompanyDbConfig());
        modelBuilder.ApplyConfiguration(new VariantPackingUnitDbConfig());
        modelBuilder.ApplyConfiguration(new VariantPackingUnitSellingPriceDbConfig());
        modelBuilder.ApplyConfiguration(new VariantSellingPriceDiscountDbConfig());
        modelBuilder.ApplyConfiguration(new VariantAttributeValueDbConfig());
    }
}
