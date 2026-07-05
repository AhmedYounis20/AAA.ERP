namespace Inventory.Domain.Models.Dtos;

public class ColorSizeCombinationDto
{
    public Guid ColorId { get; set; }
    public Guid SizeId { get; set; }
    public bool ApplyDomainChanges { get; set; } = true;
}