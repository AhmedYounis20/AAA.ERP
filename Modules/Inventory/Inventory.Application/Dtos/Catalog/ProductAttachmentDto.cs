using ERP.BuildingBlocks.Domain.Entities;
using Inventory.Domain.Models.Entities.Items;
using ERP.BuildingBlocks.Application.DTOs;
using Inventory.Domain.Models.Entities.Products;

namespace Inventory.Application.Dtos;

public class ProductAttachmentDto
{
    public Guid? Id { get; set; }
    public AttachmentType AttachmentType { get; set; }
    public string? Description { get; set; }
    public bool IsPrimary { get; set; }
    public int DisplayOrder { get; set; }
    public AttachmentDto? Attachment { get; set; }
}
