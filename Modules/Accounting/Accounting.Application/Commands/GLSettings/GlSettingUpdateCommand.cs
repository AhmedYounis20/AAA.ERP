using ERP.BuildingBlocks.Application.Commands.BaseInputModels.BaseUpdateCommands;
using Accounting.Domain.Models.Entities.GLSettings;

namespace Accounting.Application.Commands.GLSettings;

public class GlSettingUpdateCommand : BaseUpdateCommand<GLSetting>
{
    public bool IsAllowingEditVoucher { get; set; }
    public bool IsAllowingDeleteVoucher { get; set; }
    public bool IsAllowingNegativeBalances { get; set; }
    public byte DecimalDigitsNumber { get; set; }
    public byte MonthDays { get; set; }
    public DepreciationApplication DepreciationApplication { get; set; }
}
