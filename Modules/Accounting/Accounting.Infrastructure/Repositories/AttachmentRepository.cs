using Accounting.Application.Repositories.Account;
using ERP.BuildingBlocks.Persistence.Repositories;
using ERP.BuildingBlocks.Domain.Entities.Attachments;

namespace Accounting.Infrastructure.Repositories;

public class AttachmentRepository : BaseRepository<Attachment>, IAttachmentRepository
{
    public AttachmentRepository(IApplicationDbContext context) : base(context) { }
}
