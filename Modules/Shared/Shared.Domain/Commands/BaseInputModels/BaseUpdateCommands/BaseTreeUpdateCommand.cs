using Shared.Domain.BaseEntities;

namespace Shared.Domain.Commands.BaseInputModels.BaseUpdateCommands;

public class BaseTreeUpdateCommand<TResponse> : BaseUpdateCommand<TResponse>
{
    public Guid? ParentId { get; set; }
}