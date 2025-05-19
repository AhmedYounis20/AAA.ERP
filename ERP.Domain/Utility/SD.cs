using ERP.Domain.Models.Entities.Account.SubLeadgers;

namespace Domain.Account.Utility;

public static class SD
{
    public const string Role_Admin = "ADMIN";
    public const string Role_Customer  = "CUSTOMER";
    public const string BankChartAccountId = "0aecd366-95fd-47b8-8516-08dc3dfcb068";
    public const string CustomerChartOfAccountId = "a8819d15-bae1-44ab-850d-08dc3dfcb068";
    public const string CashInBoxChartOfAccountId = "bc7492d9-f338-4b46-8515-08dc3dfcb068";
    public const string SupplierChartOfAccountId = "68cf0b75-21a6-4f8a-8554-08dc3dfcb068";
    public const string BranchChartOfAccountId = "0e9722ff-df3d-4937-8510-08dc3dfcb068";
    public const string AccumlatedDepreciationId = "5b02732c-bd1f-4c9f-852a-08dc3dfcb068";
    public const string ExpensesDepreciationId = "f17c4a2d-76b2-4bb5-853d-08dc3dfcb068";

    public static readonly Dictionary<FixedAssetType, string> FixedAssetsIds  = new Dictionary<FixedAssetType, string>
    {
        { FixedAssetType.Lands, "2448c162-2329-418f-8522-08dc3dfcb068" },
        { FixedAssetType.Buildings, "66d631a5-021d-4cb5-8523-08dc3dfcb068" },
        { FixedAssetType.Equipment, "8faa0f12-3b3e-4e26-8524-08dc3dfcb068" },
        { FixedAssetType.ElectronicDevices, "c8b5cf24-98ab-414d-8525-08dc3dfcb068" },
        { FixedAssetType.OfficeSupplies, "2b70f4c6-3851-4967-8526-08dc3dfcb068" },
        { FixedAssetType.Furniture, "cb9fd3bd-720e-45f0-8527-08dc3dfcb068" },
        { FixedAssetType.VehiclesAndTransportationItems, "8f2a2956-a327-4ec0-8528-08dc3dfcb068" },
        { FixedAssetType.Tools, "97a25e9d-dff3-4412-8529-08dc3dfcb068" },
    };
    
    public static  Dictionary<FixedAssetType, string[]> FixedAssetsDepreciationDetails  = new Dictionary<FixedAssetType, string[]>
    {
        { FixedAssetType.Lands,new string[]{"",""} },
        { FixedAssetType.Buildings, new string[]{"86c0ab17-14f7-45cc-852b-08dc3dfcb068","d45672f5-5469-4349-8549-08dc3dfcb068"} },
        { FixedAssetType.Equipment, new string[]{"11756e11-9861-4158-852c-08dc3dfcb068","42155321-78a3-4933-854a-08dc3dfcb068"} },
        { FixedAssetType.ElectronicDevices, new string[]{"6d116f0e-1d2a-4924-852d-08dc3dfcb068","f6e76628-2471-4a2b-854b-08dc3dfcb068"} },
        { FixedAssetType.OfficeSupplies, new string[]{"a29e405f-8a8a-42ff-852e-08dc3dfcb068","5440be91-6ce8-4812-854c-08dc3dfcb068"} },
        { FixedAssetType.Furniture, new string[]{"aff2891b-f9e0-4824-852f-08dc3dfcb068","4cd75337-041f-4e76-854d-08dc3dfcb068"} },
        { FixedAssetType.VehiclesAndTransportationItems, new string[]{"de0d3b5a-f655-429a-8530-08dc3dfcb068","b16bff28-ee42-45cb-854e-08dc3dfcb068"} },
        { FixedAssetType.Tools, new string[]{"70b76b30-8307-481b-8531-08dc3dfcb068","52b23a54-38df-4d2f-854f-08dc3dfcb068"} },
    };
}