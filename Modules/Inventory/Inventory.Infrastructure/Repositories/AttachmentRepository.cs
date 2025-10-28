using Inventory.Application.Repositories;
using ERP.Application.Data;
using Shared.Application.Repositories;
using Shared.Domain.Attachments;

namespace Inventory.Infrastructure.Repositories;

public class AttachmentRepository : BaseRepository<Attachment>, IAttachmentRepository
{
    public AttachmentRepository(IInventoryDbContext context) : base(context) { }
}


