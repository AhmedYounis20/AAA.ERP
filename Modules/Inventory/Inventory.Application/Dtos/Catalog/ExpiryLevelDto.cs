namespace Inventory.Application.Dtos;

public class ExpiryLevelDto
{
    /// <summary>Display order / level (1-based) for frontend.</summary>
    public int Level { get; set; }
    public int DaysFrom { get; set; }
    public int DaysTo { get; set; }
    public bool EnableNotification { get; set; }
}
