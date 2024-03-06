using AAA.ERP.InputModels.BaseInputModels;
using AAA.ERP.Models.Entities.GLSettings;

namespace AAA.ERP.InputModels;

public class GLSettingInputModel : BaseInputModel
{
    public bool IsAllowingEditVoucher { get; set; }
    public bool IsAllowingDeleteVoucher { get; set; }
    public bool IsAllowingNegativeBalances { get; set; }
    public byte DecimalDigitsNumber { get; set; }
    public byte MonthDays { get; set; }
    public DepreciationApplication DepreciationApplication { get; set; }
}