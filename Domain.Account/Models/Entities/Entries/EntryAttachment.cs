using Domain.Account.Models.Entities.Attachments;
using Domain.Account.Models.Entities.Currencies;
using Domain.Account.Models.Entities.FinancialPeriods;
using Domain.Account.Models.Entities.SubLeadgers;
using Shared.BaseEntities;

namespace Domain.Account.Models.Entities.Entries;

public class EntryAttachment : BaseEntity
{
   public Guid EntryId { get; set; }
   public Guid AttachmentId { get; set; }
   public Attachment Attachment { get; set; }
}