namespace Shared.Domain.Commands.BaseInputModels.BaseCreateCommands;

public class BaseSettingCreateCommand<TResponse> : BaseCreateCommand<TResponse>
{
    public string? Name { get; set; }
    public string? NameSecondLanguage { get; set; }
}
public class BaseSettingCreateCommand : BaseCreateCommand
{
    public string? Name { get; set; }
    public string? NameSecondLanguage { get; set; }
}