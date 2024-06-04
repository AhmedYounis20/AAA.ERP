﻿using Shared.BaseEntities;

namespace Domain.Account.Models.Entities.ChartOfAccounts;

public class ChartOfAccount : BaseTreeSettingEntity<ChartOfAccount>
{
    public string? Code { get; set; }
    public Guid AccountGuidId { get; set; }
    public bool IsActiveAccount { get; set; }
    public bool IsPostedAccount { get; set; }
    public bool IsStopDealing { get; set; }
    public bool IsDepreciable { get; set; }
    public AccountNature AccountNature { get; set; }
}