using Accounting.Domain.Utility;

namespace ERP.IntegrationTests.Infrastructure;

public static class InventoryTestIds
{
    public static readonly Guid AccountGuideId = Guid.Parse("10000000-0000-0000-0000-000000000000");
    public static readonly Guid CurrencyId = Guid.Parse("10000000-0000-0000-0000-000000000001");
    public static readonly Guid FinancialPeriodId = Guid.Parse("10000000-0000-0000-0000-000000000002");
    public static readonly Guid PartyAccountId = Guid.Parse("10000000-0000-0000-0000-000000000003");
    public static readonly Guid Branch1CoaId = Guid.Parse("10000000-0000-0000-0000-000000000004");
    public static readonly Guid Branch2CoaId = Guid.Parse("10000000-0000-0000-0000-000000000005");
    public static readonly Guid StockUnderTransferCoaId = Guid.Parse(Sd.StockUnderTransferBranch);
    public static readonly Guid Branch1Id = Guid.Parse("20000000-0000-0000-0000-000000000001");
    public static readonly Guid Branch2Id = Guid.Parse("20000000-0000-0000-0000-000000000002");
    public static readonly Guid StockUnderTransferBranchId = Guid.Parse(Sd.StockUnderTransferBranch);
    public static readonly Guid ProductId = Guid.Parse("30000000-0000-0000-0000-000000000001");
    public static readonly Guid VariantId = Guid.Parse("30000000-0000-0000-0000-000000000002");
    public static readonly Guid PackingUnitId = Guid.Parse("30000000-0000-0000-0000-000000000003");
    public static readonly Guid VariantPackingUnitId = Guid.Parse("30000000-0000-0000-0000-000000000004");

    public static readonly DateTime TransactionDate = new(2026, 6, 15, 12, 0, 0, DateTimeKind.Utc);
}
