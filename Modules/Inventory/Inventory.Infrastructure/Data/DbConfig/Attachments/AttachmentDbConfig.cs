using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Shared.Domain.Attachments;
using Shared.Infrastructure.Data.DbConfig.BaseConfig;

namespace Inventory.Infrastructure.Data.DbConfig.Attachments;

public class AttachmentDbConfig : BaseEntityDbConfig<Attachment>
{

    protected override EntityTypeBuilder<Attachment> ApplyConfiguration(EntityTypeBuilder<Attachment> builder)
    {
        base.ApplyConfiguration(builder);
        builder.ToTable("Attachments");
        return builder;
    }
}


