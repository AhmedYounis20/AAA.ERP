using ERP.BuildingBlocks.Domain.Entities;
using Inventory.Domain.Models.Entities.Items;
using ERP.BuildingBlocks.Application.DTOs;
using Inventory.Domain.Models.Entities.Products;

namespace Inventory.Domain.Models.Dtos;

public class VariantAttachmentDto
{
    public Guid Id { get; set; }
    public Guid VariantId { get; set; }
    public Guid AttachmentId { get; set; }
    public AttachmentType AttachmentType { get; set; }
    public string? Description { get; set; }
    public bool IsPrimary { get; set; }
    public int DisplayOrder { get; set; }
    public AttachmentDto? Attachment { get; set; }

    public string? CreatedByName { get; set; }
    public string? CreatedByNameSecondLanguage { get; set; }
    public DateTime CreatedAt { get; set; }

    public string? ModifiedByName { get; set; }
    public string? ModifiedByNameSecondLanguage { get; set; }
    public DateTime ModifiedAt { get; set; }
}
