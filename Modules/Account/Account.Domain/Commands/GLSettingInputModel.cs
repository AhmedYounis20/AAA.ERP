using Account.Domain.Models.Entities.GLSettings;
using Shared.Domain.Commands.BaseInputModels;

namespace Account.Domain.Commands;

public class GLSettingInputModel : BaseInputModel
{
    public bool IsAllowingEditVoucher { get; set; }
    public bool IsAllowingDeleteVoucher { get; set; }
    public bool IsAllowingNegativeBalances { get; set; }
    public byte DecimalDigitsNumber { get; set; }
    public byte MonthDays { get; set; }
    public DepreciationApplication DepreciationApplication { get; set; }
}