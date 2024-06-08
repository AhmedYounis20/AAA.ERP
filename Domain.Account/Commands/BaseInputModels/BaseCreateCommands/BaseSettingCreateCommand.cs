namespace Domain.Account.Commands.BaseInputModels.BaseCreateCommands;

public class BaseSettingCreateCommand<TResponse> : BaseCreateCommand<TResponse>
{
    public string? Name { get; set; }
    public string? NameSecondLanguage { get; set; }
}