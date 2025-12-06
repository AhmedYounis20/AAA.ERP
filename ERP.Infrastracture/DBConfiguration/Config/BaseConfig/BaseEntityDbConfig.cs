using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.ValueGeneration;

namespace ERP.Infrastracture.DBConfiguration.Config.BaseConfig;

public class BaseEntityDbConfig<TEntity> : IEntityTypeConfiguration<TEntity> where TEntity : BaseEntity
{
    protected int columnNumber = 0;
    public void Configure(EntityTypeBuilder<TEntity> builder)
    {
        builder.HasKey(e => e.Id);
        _ = builder.Property(e => e.Id).HasValueGenerator<GuidValueGenerator>().HasColumnOrder(columnNumber++);
        
        // Index on CreatedAt for default ordering in pagination
        _ = builder.HasIndex(e => e.CreatedAt);
        
        _ = ApplyConfiguration(builder);
    }

    virtual protected EntityTypeBuilder<TEntity> ApplyConfiguration(EntityTypeBuilder<TEntity> builder)
    => builder;
}
