-- StockUnderTransfer identity cutover
-- Old code used Sd.StockUnderTransferBranch = 00000000-0000-0000-0000-000000000001
-- Seeded system branch/COA is always 6776b6a3-019c-480b-855b-08dc3dfcb069 ("Stock Under Transfer")
--
-- Run PRE-CHECK on every environment BEFORE go-live.
-- Run REMAP only if pre-check finds rows AND the seeded branch/COA already exist.

DECLARE @OldId uniqueidentifier = '00000000-0000-0000-0000-000000000001';
DECLARE @NewId uniqueidentifier = '6776b6a3-019c-480b-855b-08dc3dfcb069';

-- ========== PRE-CHECK ==========
SELECT 'accounting.Branches' AS [Table], COUNT(*) AS [Rows]
FROM accounting.Branches WHERE Id = @OldId OR ChartOfAccountId = @OldId
UNION ALL
SELECT 'accounting.ChartOfAccounts', COUNT(*)
FROM accounting.ChartOfAccounts WHERE Id = @OldId
UNION ALL
SELECT 'inventory.StockBalances', COUNT(*)
FROM inventory.StockBalances WHERE BranchId = @OldId
UNION ALL
SELECT 'inventory.InventoryTransfers (src/dest)', COUNT(*)
FROM inventory.InventoryTransfers
WHERE SourceBranchId = @OldId OR DestinationBranchId = @OldId;

-- If all counts are 0: no data migration needed; deploy is safe for this concern.
-- If any count > 0: ensure @NewId Branch + ChartOfAccount exist, then run REMAP below.

-- ========== REMAP (uncomment after pre-check) ==========
/*
IF NOT EXISTS (SELECT 1 FROM accounting.Branches WHERE Id = @NewId)
    THROW 50001, 'Target Stock Under Transfer branch 6776b6a3-...069 is missing. Seed/import accounts first.', 1;

IF NOT EXISTS (SELECT 1 FROM accounting.ChartOfAccounts WHERE Id = @NewId)
    THROW 50002, 'Target Stock Under Transfer COA 6776b6a3-...069 is missing. Seed/import accounts first.', 1;

BEGIN TRAN;

-- Merge stock on old sentinel into the seeded transit branch when a row already exists for the same keys.
UPDATE tgt
SET
    tgt.CurrentBalance = tgt.CurrentBalance + src.CurrentBalance,
    tgt.TotalCost = tgt.TotalCost + src.TotalCost,
    tgt.ModifiedAt = SYSUTCDATETIME()
FROM inventory.StockBalances AS tgt
INNER JOIN inventory.StockBalances AS src
    ON src.BranchId = @OldId
   AND tgt.BranchId = @NewId
   AND tgt.VariantId = src.VariantId
   AND tgt.PackingUnitId = src.PackingUnitId;

DELETE src
FROM inventory.StockBalances AS src
INNER JOIN inventory.StockBalances AS tgt
    ON src.BranchId = @OldId
   AND tgt.BranchId = @NewId
   AND tgt.VariantId = src.VariantId
   AND tgt.PackingUnitId = src.PackingUnitId;

UPDATE inventory.StockBalances
SET BranchId = @NewId, ModifiedAt = SYSUTCDATETIME()
WHERE BranchId = @OldId;

UPDATE inventory.InventoryTransfers
SET SourceBranchId = @NewId, ModifiedAt = SYSUTCDATETIME()
WHERE SourceBranchId = @OldId;

UPDATE inventory.InventoryTransfers
SET DestinationBranchId = @NewId, ModifiedAt = SYSUTCDATETIME()
WHERE DestinationBranchId = @OldId;

-- Optional: drop orphan sentinel branch/COA if present and unused.
-- DELETE FROM accounting.Branches WHERE Id = @OldId;
-- DELETE FROM accounting.ChartOfAccounts WHERE Id = @OldId;

COMMIT;
*/
