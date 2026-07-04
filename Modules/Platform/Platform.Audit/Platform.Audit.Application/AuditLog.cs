namespace Shared.BaseEntities;

/// <summary>
/// Entity for tracking changes to records (audit trail)
/// </summary>
public class AuditLog
{
    public Guid Id { get; set; }
    
    /// <summary>
    /// The entity type that was changed (e.g., "Entry", "ChartOfAccount")
    /// </summary>
    public string EntityType { get; set; } = string.Empty;
    
    /// <summary>
    /// The primary key of the entity that was changed
    /// </summary>
    public Guid EntityId { get; set; }
    
    /// <summary>
    /// The type of action performed (Create, Update, Delete)
    /// </summary>
    public AuditAction Action { get; set; }
    
    /// <summary>
    /// JSON representation of the old values (for Update and Delete)
    /// </summary>
    public string? OldValues { get; set; }
    
    /// <summary>
    /// JSON representation of the new values (for Create and Update)
    /// </summary>
    public string? NewValues { get; set; }
    
    /// <summary>
    /// List of properties that were changed (for Update only)
    /// </summary>
    public string? ChangedProperties { get; set; }
    
    /// <summary>
    /// The user who made the change
    /// </summary>
    public Guid? UserId { get; set; }
    
    /// <summary>
    /// Username for display purposes
    /// </summary>
    public string? UserName { get; set; }
    
    /// <summary>
    /// When the change occurred
    /// </summary>
    public DateTime Timestamp { get; set; }
    
    /// <summary>
    /// IP address of the request (if available)
    /// </summary>
    public string? IpAddress { get; set; }
    
    /// <summary>
    /// User agent of the request (if available)
    /// </summary>
    public string? UserAgent { get; set; }
    
    /// <summary>
    /// Additional context or notes
    /// </summary>
    public string? AdditionalInfo { get; set; }
}

/// <summary>
/// Type of audit action
/// </summary>
public enum AuditAction
{
    Create = 1,
    Update = 2,
    Delete = 3
}

