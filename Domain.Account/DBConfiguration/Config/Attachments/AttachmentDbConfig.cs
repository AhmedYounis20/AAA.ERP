using Domain.Account.DBConfiguration.Config.BaseConfig;
using Domain.Account.Models.Entities.AccountGuide;
using Domain.Account.Models.Entities.Attachments;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Domain.Account.DBConfiguration.Config.AccountGuides
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
