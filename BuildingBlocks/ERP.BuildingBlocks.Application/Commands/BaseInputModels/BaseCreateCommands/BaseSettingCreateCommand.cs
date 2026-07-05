namespace ERP.BuildingBlocks.Application.Commands.BaseInputModels.BaseCreateCommands;

public class BaseSettingCreateCommand<TResponse> : BaseCreateCommand<TResponse>
{
    public string? Name { get; set; }
    public string? NameSecondLanguage { get; set; }
}