using Shared.Domain.BaseEntities;

namespace Shared.Domain.Commands.BaseInputModels.BaseCreateCommands;

public class BaseTreeCreateCommand<TResponse> : BaseCreateCommand<TResponse>
{
    public Guid? ParentId { get; set; }
}