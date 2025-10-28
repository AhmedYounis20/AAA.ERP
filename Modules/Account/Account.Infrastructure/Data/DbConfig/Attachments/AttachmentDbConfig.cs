namespace Account.Infrastructure.Data.DbConfig.Attachments;

public class AttachmentDbConfig : BaseEntityDbConfig<Attachment>
{

    protected override EntityTypeBuilder<Attachment> ApplyConfiguration(EntityTypeBuilder<Attachment> builder)
    {
        base.ApplyConfiguration(builder);
        builder.ToTable("Attachments");
        return builder;
    }
}
