using Domain.Account.Commands.BaseInputModels.BaseCreateCommands;
using Domain.Account.Commands.BaseInputModels.BaseUpdateCommands;
using Domain.Account.Models.Entities.GLSettings;
using Shared.Responses;

namespace Domain.Account.Commands.GLSettings;

public class GlSettingUpdateCommand : BaseUpdateCommand<GLSetting>
{
    public bool IsAllowingEditVoucher { get; set; }
    public bool IsAllowingDeleteVoucher { get; set; }
    public bool IsAllowingNegativeBalances { get; set; }
    public byte DecimalDigitsNumber { get; set; }
    public byte MonthDays { get; set; }
    public DepreciationApplication DepreciationApplication { get; set; }
}