using ERP.BuildingBlocks.Application.Repositories.BaseRepositories;
using ERP.BuildingBlocks.Domain.Entities.Attachments;

namespace Accounting.Application.Repositories.Account;

/// <summary>
/// Repository contract for attachment persistence.
/// </summary>
public interface IAttachmentRepository : IBaseRepository<Attachment> { }
