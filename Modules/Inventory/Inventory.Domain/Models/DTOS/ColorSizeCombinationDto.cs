namespace Inventory.Domain.Models.DTOS;

public class ColorSizeCombinationDto
{
    public Guid ColorId { get; set; }
    public Guid SizeId { get; set; }
    public bool ApplyDomainChanges { get; set; } = true;
} 