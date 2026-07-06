using Accounting.Domain.Models.Entities.SubLeadgers;

namespace Accounting.Domain.Models.Entities.Account.SubLeadgers;

public class Tax : SubLeadgerBaseEntity<Tax>
{
    public decimal Percent { get; set; }
}
