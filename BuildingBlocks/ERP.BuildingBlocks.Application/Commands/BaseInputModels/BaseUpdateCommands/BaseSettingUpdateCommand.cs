namespace ERP.BuildingBlocks.Application.Commands.BaseInputModels.BaseUpdateCommands;

public class BaseSettingUpdateCommand<TResponse> : BaseUpdateCommand<TResponse>
{
    public string? Name { get; set; }
    public string? NameSecondLanguage { get; set; }
}