using ERP.Domain.Models.Entities.Account.Entries;
using ERP.Infrastracture.DBConfiguration.Config.BaseConfig;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ERP.Infrastracture.DBConfiguration.Config.Account.Entries
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