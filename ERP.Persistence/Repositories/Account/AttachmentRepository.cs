using ERP.Application.Repositories.Account;
using ERP.Infrastracture.Repositories.BaseRepositories;
using ERP.Shared.BaseEntities.Attachments;

namespace ERP.Infrastracture.Repositories.Account;

public class AttachmentRepository : BaseRepository<Attachment>, IAttachmentRepository
{
    public AttachmentRepository(IApplicationDbContext context) : base(context) { }
}
