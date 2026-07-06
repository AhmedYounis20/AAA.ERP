using ERP.BuildingBlocks.Persistence.Configurations;
using ERP.BuildingBlocks.Domain.Entities.Attachments;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ERP.BuildingBlocks.Persistence;

namespace Accounting.Infrastructure.Persistence.Attachments;

public class AttachmentDbConfig : BaseEntityDbConfig<Attachment>
{
    protected override EntityTypeBuilder<Attachment> ApplyConfiguration(EntityTypeBuilder<Attachment> builder)
    {
        base.ApplyConfiguration(builder);
        builder.ToTable("Attachments", DbSchemaNames.Accounting);
        return builder;
    }
}
