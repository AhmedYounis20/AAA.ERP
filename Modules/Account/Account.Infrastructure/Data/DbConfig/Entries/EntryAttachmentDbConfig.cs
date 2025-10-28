using Account.Domain.Models.Entities.Entries;

namespace Account.Infrastructure.Data.DbConfig.Entries;

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
