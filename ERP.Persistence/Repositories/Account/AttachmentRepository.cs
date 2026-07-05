using ERP.Application.Repositories.Account;
using ERP.Infrastructure.Repositories.BaseRepositories;
using ERP.Shared.BaseEntities.Attachments;

namespace ERP.Infrastructure.Repositories.Account;

public class AttachmentRepository : BaseRepository<Attachment>, IAttachmentRepository
{
    public AttachmentRepository(IApplicationDbContext context) : base(context) { }
}
