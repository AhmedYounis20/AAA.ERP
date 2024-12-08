using Domain.Account.DBConfiguration.Config.BaseConfig;
using Domain.Account.Models.Entities.Attachments;
using Domain.Account.Models.Entities.Currencies;
using Domain.Account.Models.Entities.Entries;
using Domain.Account.Models.Entities.FinancialPeriods;
using Domain.Account.Models.Entities.SubLeadgers;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Domain.Account.DBConfiguration.Config.GLSettings
{
    public class EntryAttachmentDbConfig : BaseEntityDbConfig<EntryAttachment>
    {

        protected override EntityTypeBuilder<EntryAttachment> ApplyConfiguration(EntityTypeBuilder<EntryAttachment> builder)
        {
            base.ApplyConfiguration(builder);
            builder.ToTable("EntryAttachments");

            builder.HasOne<Attachment>(e => e.Attachment)
            .WithMany()
            .HasForeignKey(e => e.AttachmentId)
            .OnDelete(DeleteBehavior.Cascade);

            return builder;
        }
    }
}