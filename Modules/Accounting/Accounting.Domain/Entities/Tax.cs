using ERP.Domain.Models.Entities.Account.SubLeadgers;

namespace Accounting.Domain.Entities;

public class Tax : SubLeadgerBaseEntity<Tax>
{
    public decimal Percent { get; set; }
}
