using Domain.Account.DBConfiguration.DbContext;
using Domain.Account.Models.Entities.AccountGuide;
using Domain.Account.Models.Entities.Attachments;
using Domain.Account.Repositories.BaseRepositories.Impelementation;
using Domain.Account.Repositories.Interfaces;

namespace Domain.Account.Repositories.Impelementation;

public class AttachmentRepository : BaseRepository<Attachment>, IAttachmentRepository
{
    public AttachmentRepository(ApplicationDbContext context) : base(context) {}
}
