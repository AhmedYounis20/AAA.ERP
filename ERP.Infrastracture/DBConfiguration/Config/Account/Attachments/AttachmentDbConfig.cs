using ERP.Domain.Models.Entities.Account.Attachments;
using ERP.Infrastracture.DBConfiguration.Config.BaseConfig;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ERP.Infrastracture.DBConfiguration.Config.Account.Attachments
{
    public class AttachmentDbConfig : BaseEntityDbConfig<Attachment>
    {

        protected override EntityTypeBuilder<Attachment> ApplyConfiguration(EntityTypeBuilder<Attachment> builder)
        {
            base.ApplyConfiguration(builder);
            builder.ToTable("Attachments");
            return builder;
        }
    }
}
