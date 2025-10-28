namespace Shared.Domain.Commands.BaseInputModels.BaseUpdateCommands;

public class BaseSettingUpdateCommand<TResponse> : BaseUpdateCommand<TResponse> 
{
    public string? Name { get; set; }
    public string? NameSecondLanguage { get; set; }
}
public class BaseSettingUpdateCommand: BaseUpdateCommand
{
    public string? Name { get; set; }
    public string? NameSecondLanguage { get; set; }
}