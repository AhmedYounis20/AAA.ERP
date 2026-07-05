public class SellingPriceDto
{
    public Guid Id { get; set; }
    public string? Name { get; set; }
    public string? NameSecondLanguage { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime ModifiedAt { get; set; }
    public bool IsDeletable { get; set; }
}