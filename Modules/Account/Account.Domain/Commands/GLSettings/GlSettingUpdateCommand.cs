using Account.Domain.Models.Entities.GLSettings;
using Shared.Domain.Commands.BaseInputModels.BaseUpdateCommands;

namespace Account.Domain.Commands.GLSettings;

public class GlSettingUpdateCommand : BaseUpdateCommand<GLSetting>
{
    public bool IsAllowingEditVoucher { get; set; }
    public bool IsAllowingDeleteVoucher { get; set; }
    public bool IsAllowingNegativeBalances { get; set; }
    public byte DecimalDigitsNumber { get; set; }
    public byte MonthDays { get; set; }
    public DepreciationApplication DepreciationApplication { get; set; }
}