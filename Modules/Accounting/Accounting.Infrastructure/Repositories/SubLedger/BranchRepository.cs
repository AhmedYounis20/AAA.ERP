using Accounting.Application.Repositories;
using Accounting.Domain.Models.Entities.SubLeadgers;
using ERP.BuildingBlocks.Domain.Entities.Attachments;
using ERP.BuildingBlocks.Persistence.Data;

namespace Accounting.Infrastructure.Repositories;

public class BranchRepository : BaseSubLeadgerRepository<Branch>, IBranchRepository
{
    private readonly IApplicationDbContext _context;

    public BranchRepository(IApplicationDbContext context) : base(context)
    {
        _context = context;
    }

    public override async Task<IEnumerable<Branch>> Get()
    {
        var branches = await _dbSet.Include(e => e.ChartOfAccount).ToListAsync();
        await HydrateLogosAsync(branches);
        return branches;
    }

    public override async Task<Branch?> Get(Guid? id)
    {
        var branch = await _dbSet.Where(e => e.Id.Equals(id)).Include(e => e.ChartOfAccount).FirstOrDefaultAsync();
        if (branch is not null)
            await HydrateLogosAsync([branch]);
        return branch;
    }

    public override async Task<List<Branch>> GetLevel(int level = 0)
    {
        List<Branch> entities = await _dbSet.Include(e => e.ChartOfAccount).Where(e => e.ParentId == null).ToListAsync();
        await HydrateLogosAsync(entities);
        if (level == 0)
            return entities;

        return await GetChildren(entities, level - 1);
    }

    public override async Task<List<Branch>> GetChildren(Guid id, int level = 1)
    {
        List<Branch> children = await _dbSet.Where(e => e.ParentId.Equals(id)).Include(e => e.ChartOfAccount).ToListAsync();
        await HydrateLogosAsync(children);
        if (level == 0)
            return children;

        return await GetChildren(children, level - 1);
    }

    private async Task HydrateLogosAsync(IEnumerable<Branch> branches)
    {
        var list = branches.ToList();
        var ids = list.Where(branch => branch.AttachmentId.HasValue)
            .Select(branch => branch.AttachmentId!.Value)
            .Distinct()
            .ToList();
        if (ids.Count == 0)
            return;

        var rows = await _context.Set<Attachment>()
            .AsNoTracking()
            .Where(attachment => ids.Contains(attachment.Id))
            .Select(attachment => new
            {
                attachment.Id,
                attachment.FileName,
                attachment.FileContentType,
                attachment.CreatedAt,
                attachment.ModifiedAt
            })
            .ToListAsync();

        var map = rows.ToDictionary(row => row.Id);
        foreach (var branch in list)
        {
            if (branch.AttachmentId is Guid attachmentId && map.TryGetValue(attachmentId, out var row))
            {
                branch.Attachment = new Attachment
                {
                    Id = row.Id,
                    FileName = row.FileName,
                    FileContentType = row.FileContentType,
                    CreatedAt = row.CreatedAt,
                    ModifiedAt = row.ModifiedAt
                };
            }
        }
    }
}
