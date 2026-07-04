using Shared.BaseEntities;
using Shared.DTOs;
using Shared.Responses;

namespace ERP.Application.Services.Audit;

/// <summary>
/// Service for managing audit logs
/// </summary>
public interface IAuditService
{
    /// <summary>
    /// Creates an audit log entry
    /// </summary>
    Task LogAsync(AuditLog auditLog);
    
    /// <summary>
    /// Creates an audit log entry for entity creation
    /// </summary>
    Task LogCreateAsync<TEntity>(TEntity entity, Guid? userId = null, string? userName = null) where TEntity : BaseEntity;
    
    /// <summary>
    /// Creates an audit log entry for entity update
    /// </summary>
    Task LogUpdateAsync<TEntity>(TEntity oldEntity, TEntity newEntity, Guid? userId = null, string? userName = null) where TEntity : BaseEntity;
    
    /// <summary>
    /// Creates an audit log entry for entity deletion
    /// </summary>
    Task LogDeleteAsync<TEntity>(TEntity entity, Guid? userId = null, string? userName = null) where TEntity : BaseEntity;
    
    /// <summary>
    /// Gets audit logs for a specific entity
    /// </summary>
    Task<ApiResponse<PaginatedResult<AuditLog>>> GetAuditLogsForEntity(
        string entityType,
        Guid entityId,
        int pageNumber = 1,
        int pageSize = 10,
        CancellationToken cancellationToken = default);
    
    /// <summary>
    /// Gets audit logs with filtering
    /// </summary>
    Task<ApiResponse<PaginatedResult<AuditLog>>> GetAuditLogs(
        AuditLogFilterDto filter,
        CancellationToken cancellationToken = default);
    
    /// <summary>
    /// Gets a specific audit log by ID
    /// </summary>
    Task<ApiResponse<AuditLog>> GetAuditLogById(Guid id);
}

/// <summary>
/// Filter DTO for audit logs
/// </summary>
public class AuditLogFilterDto : BaseFilterDto
{
    /// <summary>
    /// Filter by entity type
    /// </summary>
    public string? EntityType { get; set; }
    
    /// <summary>
    /// Filter by entity ID
    /// </summary>
    public Guid? EntityId { get; set; }
    
    /// <summary>
    /// Filter by action type
    /// </summary>
    public AuditAction? Action { get; set; }
    
    /// <summary>
    /// Filter by user ID
    /// </summary>
    public Guid? UserId { get; set; }
    
    /// <summary>
    /// Filter by timestamp from
    /// </summary>
    public DateTime? TimestampFrom { get; set; }
    
    /// <summary>
    /// Filter by timestamp to
    /// </summary>
    public DateTime? TimestampTo { get; set; }
}

