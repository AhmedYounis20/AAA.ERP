using Domain.Account.InputModels.BaseInputModels;
using Accounting.Domain.Models.Entities.GLSettings;

namespace ERP.Domain.Commands.Account;

public class GLSettingInputModel : BaseInputModel
{
    public bool IsAllowingEditVoucher { get; set; }
    public bool IsAllowingDeleteVoucher { get; set; }
    public bool IsAllowingNegativeBalances { get; set; }
    public byte DecimalDigitsNumber { get; set; }
    public byte MonthDays { get; set; }
    public DepreciationApplication DepreciationApplication { get; set; }
}