using Domain.Account.Models.Entities.Attachments;
using ERP.Infrastracture.Repositories.BaseRepositories;

namespace ERP.Infrastracture.Repositories.Account;

public class AttachmentRepository : BaseRepository<Attachment>, IAttachmentRepository
{
    public AttachmentRepository(ApplicationDbContext context) : base(context) { }
}
