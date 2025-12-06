using ERP.Application.Services.Audit;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ERP.API.Controllers.Audit;

/// <summary>
/// Controller for viewing audit logs
/// </summary>
[Route("api/[controller]")]
[ApiController]
[Authorize]
public class AuditLogsController : ControllerBase
{
    private readonly IAuditService _auditService;

    public AuditLogsController(IAuditService auditService)
    {
        _auditService = auditService;
    }

    /// <summary>
    /// Gets paginated audit logs with filtering
    /// </summary>
    [HttpGet]
    public async Task<IActionResult> GetAuditLogs([FromQuery] AuditLogFilterDto filter, CancellationToken cancellationToken)
    {
        var result = await _auditService.GetAuditLogs(filter, cancellationToken);
        return StatusCode((int)result.StatusCode, result);
    }

    /// <summary>
    /// Gets audit log by ID
    /// </summary>
    [HttpGet("{id}")]
    public async Task<IActionResult> GetAuditLogById(Guid id)
    {
        var result = await _auditService.GetAuditLogById(id);
        return StatusCode((int)result.StatusCode, result);
    }

    /// <summary>
    /// Gets audit logs for a specific entity
    /// </summary>
    [HttpGet("entity/{entityType}/{entityId}")]
    public async Task<IActionResult> GetAuditLogsForEntity(
        string entityType,
        Guid entityId,
        [FromQuery] int pageNumber = 1,
        [FromQuery] int pageSize = 10,
        CancellationToken cancellationToken = default)
    {
        var result = await _auditService.GetAuditLogsForEntity(entityType, entityId, pageNumber, pageSize, cancellationToken);
        return StatusCode((int)result.StatusCode, result);
    }
}

