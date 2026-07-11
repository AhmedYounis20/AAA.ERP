namespace Accounting.Contracts.Integration;

public static class EntrySourceDocumentTypes
{
    public const string InventoryTransaction = "InventoryTransaction";
    public const string InventoryTransferStage = "InventoryTransferStage";
    public const string InventoryTransferComplete = "InventoryTransferComplete";
    public const string InventoryTransferReject = "InventoryTransferReject";
}
