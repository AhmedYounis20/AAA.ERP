using ERP.Application.Data;
using ERP.Application.Services.Audit;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Shared.BaseEntities;
using Shared.DTOs;
using Shared.Responses;
using System.Net;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace ERP.Infrastracture.Services.Audit;

/// <summary>
/// Implementation of the audit service
/// </summary>
public class AuditService : IAuditService
{
    private readonly IApplicationDbContext _dbContext;
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly JsonSerializerOptions _jsonOptions;

    public AuditService(IApplicationDbContext dbContext, IHttpContextAccessor httpContextAccessor)
    {
        _dbContext = dbContext;
        _httpContextAccessor = httpContextAccessor;
        _jsonOptions = new JsonSerializerOptions
        {
            WriteIndented = false,
            ReferenceHandler = ReferenceHandler.IgnoreCycles,
            DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull
        };
    }

    public async Task LogAsync(AuditLog auditLog)
    {
        auditLog.Id = Guid.NewGuid();
        auditLog.Timestamp = DateTime.UtcNow;
        
        // Add request context if available
        if (_httpContextAccessor.HttpContext != null)
        {
            auditLog.IpAddress ??= GetIpAddress();
            auditLog.UserAgent ??= _httpContextAccessor.HttpContext.Request.Headers["User-Agent"].FirstOrDefault();
        }

        await _dbContext.Set<AuditLog>().AddAsync(auditLog);
        await _dbContext.SaveChangesAsync();
    }

    public async Task LogCreateAsync<TEntity>(TEntity entity, Guid? userId = null, string? userName = null) 
        where TEntity : BaseEntity
    {
        var auditLog = new AuditLog
        {
            EntityType = typeof(TEntity).Name,
            EntityId = entity.Id,
            Action = AuditAction.Create,
            NewValues = SerializeEntity(entity),
            UserId = userId,
            UserName = userName
        };

        await LogAsync(auditLog);
    }

    public async Task LogUpdateAsync<TEntity>(TEntity oldEntity, TEntity newEntity, Guid? userId = null, string? userName = null) 
        where TEntity : BaseEntity
    {
        var changedProperties = GetChangedProperties(oldEntity, newEntity);
        
        var auditLog = new AuditLog
        {
            EntityType = typeof(TEntity).Name,
            EntityId = newEntity.Id,
            Action = AuditAction.Update,
            OldValues = SerializeEntity(oldEntity),
            NewValues = SerializeEntity(newEntity),
            ChangedProperties = string.Join(", ", changedProperties),
            UserId = userId,
            UserName = userName
        };

        await LogAsync(auditLog);
    }

    public async Task LogDeleteAsync<TEntity>(TEntity entity, Guid? userId = null, string? userName = null) 
        where TEntity : BaseEntity
    {
        var auditLog = new AuditLog
        {
            EntityType = typeof(TEntity).Name,
            EntityId = entity.Id,
            Action = AuditAction.Delete,
            OldValues = SerializeEntity(entity),
            UserId = userId,
            UserName = userName
        };

        await LogAsync(auditLog);
    }

    public async Task<ApiResponse<PaginatedResult<AuditLog>>> GetAuditLogsForEntity(
        string entityType,
        Guid entityId,
        int pageNumber = 1,
        int pageSize = 10,
        CancellationToken cancellationToken = default)
    {
        try
        {
            var query = _dbContext.Set<AuditLog>()
                .Where(a => a.EntityType == entityType && a.EntityId == entityId)
                .OrderByDescending(a => a.Timestamp);

            var result = await query.ToPaginatedResultAsync(pageNumber, pageSize, cancellationToken);

            return new ApiResponse<PaginatedResult<AuditLog>>
            {
                IsSuccess = true,
                StatusCode = HttpStatusCode.OK,
                Result = result
            };
        }
        catch (Exception ex)
        {
            return new ApiResponse<PaginatedResult<AuditLog>>
            {
                IsSuccess = false,
                StatusCode = HttpStatusCode.InternalServerError,
                Errors = new List<MessageTemplate> { new() { MessageKey = ex.Message } }
            };
        }
    }

    public async Task<ApiResponse<PaginatedResult<AuditLog>>> GetAuditLogs(
        AuditLogFilterDto filter,
        CancellationToken cancellationToken = default)
    {
        try
        {
            var query = _dbContext.Set<AuditLog>().AsQueryable();

            // Apply filters
            if (!string.IsNullOrEmpty(filter.EntityType))
                query = query.Where(a => a.EntityType == filter.EntityType);

            if (filter.EntityId.HasValue)
                query = query.Where(a => a.EntityId == filter.EntityId.Value);

            if (filter.Action.HasValue)
                query = query.Where(a => a.Action == filter.Action.Value);

            if (filter.UserId.HasValue)
                query = query.Where(a => a.UserId == filter.UserId.Value);

            if (filter.TimestampFrom.HasValue)
                query = query.Where(a => a.Timestamp >= filter.TimestampFrom.Value);

            if (filter.TimestampTo.HasValue)
                query = query.Where(a => a.Timestamp <= filter.TimestampTo.Value);

            if (!string.IsNullOrEmpty(filter.SearchTerm))
                query = query.Where(a => 
                    a.EntityType.Contains(filter.SearchTerm) || 
                    (a.UserName != null && a.UserName.Contains(filter.SearchTerm)));

            // Apply sorting
            query = filter.SortDescending 
                ? query.OrderByDescending(a => a.Timestamp) 
                : query.OrderBy(a => a.Timestamp);

            var result = await query.ToPaginatedResultAsync(filter.PageNumber, filter.PageSize, cancellationToken);

            return new ApiResponse<PaginatedResult<AuditLog>>
            {
                IsSuccess = true,
                StatusCode = HttpStatusCode.OK,
                Result = result
            };
        }
        catch (Exception ex)
        {
            return new ApiResponse<PaginatedResult<AuditLog>>
            {
                IsSuccess = false,
                StatusCode = HttpStatusCode.InternalServerError,
                Errors = new List<MessageTemplate> { new() { MessageKey = ex.Message } }
            };
        }
    }

    public async Task<ApiResponse<AuditLog>> GetAuditLogById(Guid id)
    {
        try
        {
            var auditLog = await _dbContext.Set<AuditLog>().FindAsync(id);

            if (auditLog == null)
            {
                return new ApiResponse<AuditLog>
                {
                    IsSuccess = false,
                    StatusCode = HttpStatusCode.NotFound,
                    Errors = new List<MessageTemplate> { new() { MessageKey = "AuditLogNotFound" } }
                };
            }

            return new ApiResponse<AuditLog>
            {
                IsSuccess = true,
                StatusCode = HttpStatusCode.OK,
                Result = auditLog
            };
        }
        catch (Exception ex)
        {
            return new ApiResponse<AuditLog>
            {
                IsSuccess = false,
                StatusCode = HttpStatusCode.InternalServerError,
                Errors = new List<MessageTemplate> { new() { MessageKey = ex.Message } }
            };
        }
    }

    private string SerializeEntity<TEntity>(TEntity entity)
    {
        try
        {
            return JsonSerializer.Serialize(entity, _jsonOptions);
        }
        catch
        {
            return "{}";
        }
    }

    private static List<string> GetChangedProperties<TEntity>(TEntity oldEntity, TEntity newEntity)
    {
        var changedProperties = new List<string>();
        var properties = typeof(TEntity).GetProperties()
            .Where(p => p.CanRead && p.PropertyType.IsValueType || p.PropertyType == typeof(string));

        foreach (var property in properties)
        {
            var oldValue = property.GetValue(oldEntity);
            var newValue = property.GetValue(newEntity);

            if (!Equals(oldValue, newValue))
            {
                changedProperties.Add(property.Name);
            }
        }

        return changedProperties;
    }

    private string? GetIpAddress()
    {
        var context = _httpContextAccessor.HttpContext;
        if (context == null) return null;

        // Check for forwarded header first (for proxy scenarios)
        var forwardedFor = context.Request.Headers["X-Forwarded-For"].FirstOrDefault();
        if (!string.IsNullOrEmpty(forwardedFor))
        {
            return forwardedFor.Split(',').FirstOrDefault()?.Trim();
        }

        return context.Connection.RemoteIpAddress?.ToString();
    }
}

