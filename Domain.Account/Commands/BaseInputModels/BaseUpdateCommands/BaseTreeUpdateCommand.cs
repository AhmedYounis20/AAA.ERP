namespace Domain.Account.Commands.BaseInputModels.BaseUpdateCommands;

public class BaseTreeUpdateCommand<TCommand> : BaseUpdateCommand<TCommand>
{
    public Guid? ParentId { get; set; }
}