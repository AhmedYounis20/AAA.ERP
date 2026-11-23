using ERP.BuildingBlocks.Application.Attachments;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Platform.Audit.Api.Controllers;

/// <summary>Streams attachment bytes. Business responses expose this URL and do not embed the file.</summary>
[Route("api/attachments")]
[ApiController]
[Authorize]
public class AttachmentsController(IAttachmentStore store) : ControllerBase
{
    [HttpGet("{id:guid}/content")]
    public async Task<IActionResult> GetContent(Guid id, CancellationToken cancellationToken)
    {
        var content = await store.OpenReadAsync(id, cancellationToken);
        if (content is null)
            return NotFound();

        return File(content.Data, content.ContentType, content.FileName);
    }
}
