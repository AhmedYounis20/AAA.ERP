using ERP.Infrastructure.DBConfiguration.Config.BaseConfig;
using ERP.Shared.BaseEntities.Attachments;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ERP.BuildingBlocks.Persistence;

namespace ERP.Infrastructure.DBConfiguration.Config.Account.Attachments
{
    public class AttachmentDbConfig : BaseEntityDbConfig<Attachment>
    {

        protected override EntityTypeBuilder<Attachment> ApplyConfiguration(EntityTypeBuilder<Attachment> builder)
        {
            base.ApplyConfiguration(builder);
            builder.ToTable("Attachments", DbSchemaNames.Accounting);
            return builder;
        }
    }
}
