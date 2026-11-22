#!/usr/bin/env python3
import json
from pathlib import Path

path = Path(__file__).resolve().parents[1] / "ERP.Host/seeding/account/ChartOfAccounts.json"
accounts = json.loads(path.read_text())

mapping = {
    "0aecd366-95fd-47b8-8516-08dc3dfcb068": ("Bank", None, "Default"),
    "a8819d15-bae1-44ab-850d-08dc3dfcb068": ("Customer", None, "Default"),
    "bc7492d9-f338-4b46-8515-08dc3dfcb068": ("CashInBox", None, "Default"),
    "68cf0b75-21a6-4f8a-8554-08dc3dfcb068": ("Supplier", None, "Default"),
    "0e9722ff-df3d-4937-8510-08dc3dfcb068": ("Branch", None, "Default"),
    "6776b6a3-019c-480b-855b-08dc3dfcb069": ("Branch", None, "StockUnderTransfer"),
    "2448c162-2329-418f-8522-08dc3dfcb068": ("FixedAsset", "Lands", "Asset"),
    "66d631a5-021d-4cb5-8523-08dc3dfcb068": ("FixedAsset", "Buildings", "Asset"),
    "8faa0f12-3b3e-4e26-8524-08dc3dfcb068": ("FixedAsset", "Equipment", "Asset"),
    "c8b5cf24-98ab-414d-8525-08dc3dfcb068": ("FixedAsset", "ElectronicDevices", "Asset"),
    "2b70f4c6-3851-4967-8526-08dc3dfcb068": ("FixedAsset", "OfficeSupplies", "Asset"),
    "cb9fd3bd-720e-45f0-8527-08dc3dfcb068": ("FixedAsset", "Furniture", "Asset"),
    "8f2a2956-a327-4ec0-8528-08dc3dfcb068": ("FixedAsset", "VehiclesAndTransportationItems", "Asset"),
    "97a25e9d-dff3-4412-8529-08dc3dfcb068": ("FixedAsset", "Tools", "Asset"),
    "86c0ab17-14f7-45cc-852b-08dc3dfcb068": ("FixedAsset", "Buildings", "AccumulatedDepreciation"),
    "11756e11-9861-4158-852c-08dc3dfcb068": ("FixedAsset", "Equipment", "AccumulatedDepreciation"),
    "6d116f0e-1d2a-4924-852d-08dc3dfcb068": ("FixedAsset", "ElectronicDevices", "AccumulatedDepreciation"),
    "a29e405f-8a8a-42ff-852e-08dc3dfcb068": ("FixedAsset", "OfficeSupplies", "AccumulatedDepreciation"),
    "aff2891b-f9e0-4824-852f-08dc3dfcb068": ("FixedAsset", "Furniture", "AccumulatedDepreciation"),
    "de0d3b5a-f655-429a-8530-08dc3dfcb068": ("FixedAsset", "VehiclesAndTransportationItems", "AccumulatedDepreciation"),
    "70b76b30-8307-481b-8531-08dc3dfcb068": ("FixedAsset", "Tools", "AccumulatedDepreciation"),
    "d45672f5-5469-4349-8549-08dc3dfcb068": ("FixedAsset", "Buildings", "DepreciationExpense"),
    "42155321-78a3-4933-854a-08dc3dfcb068": ("FixedAsset", "Equipment", "DepreciationExpense"),
    "f6e76628-2471-4a2b-854b-08dc3dfcb068": ("FixedAsset", "ElectronicDevices", "DepreciationExpense"),
    "5440be91-6ce8-4812-854c-08dc3dfcb068": ("FixedAsset", "OfficeSupplies", "DepreciationExpense"),
    "4cd75337-041f-4e76-854d-08dc3dfcb068": ("FixedAsset", "Furniture", "DepreciationExpense"),
    "b16bff28-ee42-45cb-854e-08dc3dfcb068": ("FixedAsset", "VehiclesAndTransportationItems", "DepreciationExpense"),
    "52b23a54-38df-4d2f-854f-08dc3dfcb068": ("FixedAsset", "Tools", "DepreciationExpense"),
}

updated = 0
for account in accounts:
    account_id = account.get("Id")
    if account_id in mapping:
        sub_type, fa_type, role = mapping[account_id]
        account["IsSubLeadgerBaseAccount"] = True
        account["SubLeadgerType"] = sub_type
        account["SubLedgerAccountRole"] = role
        if fa_type:
            account["FixedAssetType"] = fa_type
        updated += 1

path.write_text(json.dumps(accounts, indent=2, ensure_ascii=False) + "\n")
print(f"Updated {updated} seed accounts")
