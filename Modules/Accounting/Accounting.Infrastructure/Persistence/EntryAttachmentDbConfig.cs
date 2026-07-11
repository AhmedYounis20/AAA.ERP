using Accounting.Domain.Models.Entities.Entries;
using ERP.BuildingBlocks.Persistence.Configurations;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ERP.BuildingBlocks.Persistence;

namespace Accounting.Infrastructure.Persistence
{
    public class EntryAttachmentDbConfig : BaseEntityDbConfig<EntryAttachment>
    {

        protected override EntityTypeBuilder<EntryAttachment> ApplyConfiguration(EntityTypeBuilder<EntryAttachment> builder)
        {
            base.ApplyConfiguration(builder);
            builder.ToTable("EntryAttachments", DbSchemaNames.Accounting);

            builder.HasOne(e => e.Attachment)
            .WithMany()
            .HasForeignKey(e => e.AttachmentId)
            .OnDelete(DeleteBehavior.Cascade);

            return builder;
        }
    }
}