using Accounting.Domain.Models.Entities.SubLeadgers;

namespace Accounting.Domain.Models.Entities.SubLeadgers;

public class Tax : SubLeadgerBaseEntity<Tax>
{
    public decimal Percent { get; set; }
}
