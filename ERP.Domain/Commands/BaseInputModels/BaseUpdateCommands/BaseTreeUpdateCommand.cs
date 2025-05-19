namespace Domain.Account.Commands.BaseInputModels.BaseUpdateCommands;

public class BaseTreeUpdateCommand<TResponse> : BaseUpdateCommand<TResponse>
{
    public Guid? ParentId { get; set; }
}