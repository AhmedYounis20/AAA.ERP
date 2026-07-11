namespace ERP.BuildingBlocks.Application.CQRS.Handlers.Execution;

/// <summary>
/// Controls generic handler execution. Replaces the legacy <c>isValidate</c> flag on BaseService methods.
/// Used by workflow services when invoking handler logic internally (future phases).
/// </summary>
public sealed class CrudExecutionOptions
{
    public static CrudExecutionOptions Default { get; } = new();

    /// <summary>
    /// When true, skips <c>Validate*Async</c> business hooks (equivalent to <c>isValidate: false</c>).
    /// </summary>
    public bool SuppressBusinessValidation { get; init; }

    /// <summary>
    /// When true, skips <c>Before*</c> and <c>After*</c> hooks. For internal orchestration only.
    /// </summary>
    public bool SuppressHooks { get; init; }
}
