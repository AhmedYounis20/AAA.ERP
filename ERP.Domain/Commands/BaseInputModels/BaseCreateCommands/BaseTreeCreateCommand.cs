using Domain.Account.InputModels.BaseInputModels;

namespace Domain.Account.Commands.BaseInputModels.BaseCreateCommands;

public class BaseTreeCreateCommand<TResponse> : BaseCreateCommand<TResponse>
{
    public Guid? ParentId { get; set; }
}