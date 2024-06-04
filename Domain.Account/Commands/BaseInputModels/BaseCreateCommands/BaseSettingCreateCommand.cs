namespace Domain.Account.Commands.BaseInputModels.BaseCreateCommands;

public class BaseSettingInputModel<TCommand> : BaseInputModel<TCommand>
{
    public string? Name { get; set; }
    public string? NameSecondLanguage { get; set; }
}