using Domain.Account.InputModels.BaseInputModels;

namespace Domain.Account.Commands.BaseInputModels.BaseCreateCommands;

public class BaseTreeInputModel<TCommand> : BaseInputModel<TCommand>
{
    public Guid? ParentId { get; set; }
}