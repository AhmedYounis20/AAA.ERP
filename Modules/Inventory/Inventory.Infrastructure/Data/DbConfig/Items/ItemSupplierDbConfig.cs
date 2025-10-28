namespace Inventory.Infrastructure.Data.DbConfig.Items;

public class ItemSupplierDbConfig : BaseEntityDbConfig<ItemSupplier>
{
    protected override EntityTypeBuilder<ItemSupplier> ApplyConfiguration(EntityTypeBuilder<ItemSupplier> builder)
    {
        base.ApplyConfiguration(builder);
        builder.ToTable("ItemSuppliers");

        _ = builder.HasIndex(e => new { e.ItemId, e.SupplierId }).IsUnique();

        //_ = builder.HasOne(e => e.Supplier)
        //.WithMany()
        //.HasForeignKey(e => e.SupplierId)
        //.OnDelete(DeleteBehavior.Restrict);

        return builder;
    }
}
