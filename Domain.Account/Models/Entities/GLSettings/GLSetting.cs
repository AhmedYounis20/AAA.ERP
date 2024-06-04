using AAA.ERP.Models.BaseEntities;

namespace AAA.ERP.Models.Entities.GLSettings;

public class GLSetting : BaseEntity
{
    public bool IsAllowingEditVoucher { get; set; }
    public bool IsAllowingDeleteVoucher { get; set; }
    public bool IsAllowingNegativeBalances { get; set; }
    public byte DecimalDigitsNumber { get; set; }
    public byte MonthDays { get; set; }
    public DepreciationApplication DepreciationApplication { get; set; }
}