using Accounting.Domain.Models.Entities.SubLeadgers;

namespace Accounting.Domain.Entities;

public class Tax : SubLeadgerBaseEntity<Tax>
{
    public decimal Percent { get; set; }
}
