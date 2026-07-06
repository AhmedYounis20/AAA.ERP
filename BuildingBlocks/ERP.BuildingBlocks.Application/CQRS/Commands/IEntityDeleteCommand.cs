namespace ERP.BuildingBlocks.Application.CQRS.Commands;

/// <summary>
/// Marker for delete commands processed by <see cref="Handlers.Base.BaseDeleteHandler{TEntity,TDeleteCommand}"/>.
/// Module delete commands should implement this interface during CQRS migration.
/// </summary>
public interface IEntityDeleteCommand
{
    Guid Id { get; }
}
