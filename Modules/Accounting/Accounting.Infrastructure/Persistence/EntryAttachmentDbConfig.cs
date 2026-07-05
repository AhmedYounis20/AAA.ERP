using Accounting.Domain.Models.Entities.Entries;
using ERP.Infrastructure.DBConfiguration.Config.BaseConfig;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Accounting.Infrastructure.Persistence
{
    public class EntryAttachmentDbConfig : BaseEntityDbConfig<EntryAttachment>
    {

        protected override EntityTypeBuilder<EntryAttachment> ApplyConfiguration(EntityTypeBuilder<EntryAttachment> builder)
        {
            base.ApplyConfiguration(builder);
            builder.ToTable("EntryAttachments");

            builder.HasOne(e => e.Attachment)
            .WithMany()
            .HasForeignKey(e => e.AttachmentId)
            .OnDelete(DeleteBehavior.Cascade);

            return builder;
        }
    }
}