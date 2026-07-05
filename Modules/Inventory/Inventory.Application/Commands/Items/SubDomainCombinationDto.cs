using ERP.Domain.Models.Entities.Inventory.Items;

namespace Inventory.Application.Commands.Items;

public class SubDomainCombinationDto
{
    public Guid ColorId { get; set; }
    public Guid SizeId { get; set; }
    public bool ApplyDomainChanges { get; set; } = true;
} 