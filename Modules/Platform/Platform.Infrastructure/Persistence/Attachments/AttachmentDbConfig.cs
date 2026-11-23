using ERP.BuildingBlocks.Domain.Entities.Attachments;
using ERP.BuildingBlocks.Persistence;
using ERP.BuildingBlocks.Persistence.Configurations;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Platform.Infrastructure.Persistence.Attachments;

public class AttachmentDbConfig : BaseEntityDbConfig<Attachment>
{
    protected override EntityTypeBuilder<Attachment> ApplyConfiguration(EntityTypeBuilder<Attachment> builder)
    {
        base.ApplyConfiguration(builder);
        builder.ToTable("Attachments", DbSchemaNames.Platform);
        builder.Property(e => e.StorageProvider).HasConversion<int>();
        builder.Ignore(e => e.ContentUrl);
        builder.Ignore(e => e.AttachmentId);
        return builder;
    }
}
