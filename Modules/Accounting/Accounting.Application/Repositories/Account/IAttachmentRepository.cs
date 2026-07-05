using ERP.BuildingBlocks.Application.Repositories.BaseRepositories;
using ERP.Shared.BaseEntities.Attachments;

namespace Accounting.Application.Repositories.Account;

/// <summary>
/// Repository contract for attachment persistence.
/// </summary>
public interface IAttachmentRepository : IBaseRepository<Attachment> { }
