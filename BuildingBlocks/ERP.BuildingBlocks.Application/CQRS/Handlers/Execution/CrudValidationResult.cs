namespace ERP.BuildingBlocks.Application.CQRS.Handlers.Execution;

public readonly record struct CrudValidationResult(bool IsValid, IReadOnlyList<string> Errors)
{
    public static CrudValidationResult Success { get; } = new(true, Array.Empty<string>());

    public static CrudValidationResult Failure(params string[] errors) => new(false, errors);

    public static CrudValidationResult Failure(IEnumerable<string> errors) =>
        new(false, errors.ToList());
}

/// <summary>
/// Validation result for operations that load an entity (update, delete).
/// </summary>
public readonly record struct CrudEntityValidationResult<TEntity>(
    bool IsValid,
    IReadOnlyList<string> Errors,
    TEntity? Entity)
    where TEntity : class
{
    public static CrudEntityValidationResult<TEntity> Success(TEntity entity) =>
        new(true, Array.Empty<string>(), entity);

    public static CrudEntityValidationResult<TEntity> Failure(IEnumerable<string> errors, TEntity? entity = null) =>
        new(false, errors.ToList(), entity);
}
