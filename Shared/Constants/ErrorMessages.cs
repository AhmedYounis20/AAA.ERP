namespace Shared.Constants;

/// <summary>
/// Centralized error message keys for consistency
/// </summary>
public static class ErrorMessages
{
    // Generic errors
    public const string RecordNotFound = "RecordNotFound";
    public const string RecordAlreadyExists = "RecordAlreadyExists";
    public const string InvalidOperation = "InvalidOperation";
    public const string UnauthorizedAccess = "UnauthorizedAccess";
    public const string Unauthorized = "Unauthorized";
    public const string ValidationFailed = "ValidationFailed";
    public const string InternalServerError = "InternalServerError";
    public const string UnexpectedError = "UnexpectedError";

    // CRUD operations
    public const string CreateFailed = "CreateFailed";
    public const string UpdateFailed = "UpdateFailed";
    public const string DeleteFailed = "DeleteFailed";
    public const string DuplicateRecord = "DuplicateRecord";

    // Financial operations
    public const string FinancialPeriodNotFound = "NotFoundCurrentFinancialPeriod";
    public const string EntryNumberExists = "ExistedEntryWithSameNumber";
    public const string InvalidFinancialPeriod = "InvalidFinancialPeriod";
    public const string BalanceNotZero = "BalanceNotZero";

    // Chart of Accounts
    public const string ChartOfAccountCodeExists = "ChartOfAccoutWithSameCodeExist";
    public const string ChartOfAccountCodeCannotChange = "ChartOfAccountCodeCannotBeChanged";
    public const string ChartOfAccountFromSubLeadger = "ChartOfAccountCreatedFomSubLeadger";
    public const string ChartOfAccountIsSubLeadgerBase = "ChartOfAccountBaseSubLeadgerAccount";

    // Inventory
    public const string ItemCodeDuplicated = "ITEM_CODE_IS_DUPLICATED";
    public const string ItemBarcodeDuplicated = "ITEM_ONE_OF_BARCODES_IS_DUPLICATED";
    public const string SupplierNotFound = "ONE_OF_SUPPLIERS_NOT_FOUND";
    public const string ManufacturerNotFound = "ONE_OF_MANUFACTURER_COMPANIES_NOT_FOUND";
    public const string CannotSetSubDomainAsParent = "CANNOT_SET_SUBDOMAIN_AS_PARENT";
    public const string InsufficientStock = "INSUFFICIENT_STOCK";

    // Validation messages
    public const string RequiredField = "RequiredField";
    public const string InvalidFormat = "InvalidFormat";
    public const string MaxLengthExceeded = "MaxLengthExceeded";
    public const string MinLengthNotMet = "MinLengthNotMet";
    public const string InvalidRange = "InvalidRange";

    // Service / domain message keys
    public const string AttributeValueNotFound = "AttributeValueNotFound";
    public const string BranchOrBranchAccountNotFound = "BranchOrBranchAccountNotFound";
    public const string CannotChangeNodeType = "CannotChangeNodeType";
    public const string NodeTypeCannotBeChanged = "Node Type cannot be changed";
    public const string ChartOfAccountUpdateSubLeadgerName = "ChartOfAccountUpdateSubLeadgerName";
    public const string ChartOfAccountUpdateSubLeadgerNameSecondLanguage = "ChartOfAccountUpdateSubLeadgerNameSecondLanguage";
    public const string ColorValueIsDuplicate = "ColorValueIsDuplicate";
    public const string CurrencySymbolIsExisted = "CurrencySymbolIsExisted";
    public const string DefaultCurrencyCannotBeDeleted = "DefaultCurrencyCannotBeDeleted";
    public const string DefaultCurrencyIsAlreadyExistedWithName = "DefaultCurrencyIsAlreadyExistedWithName";
    public const string DefaultCurrencyNotFound = "DefaultCurrencyNotFound";
    public const string DefaultPackingUnitNotFound = "DefaultPackingUnitNotFound";
    public const string DistinationBranchNotFound = "DistinationBranchNotFound";
    public const string EXPIRY_LEVEL_DAYS_FROM_MUST_BE_LESS_THAN_DAYS_TO = "EXPIRY_LEVEL_DAYS_FROM_MUST_BE_LESS_THAN_DAYS_TO";
    public const string EXPIRY_LEVEL_RANGES_MUST_NOT_OVERLAP = "EXPIRY_LEVEL_RANGES_MUST_NOT_OVERLAP";
    public const string FaildToGenerateEntryNubmer = "FaildToGenerateEntryNubmer";
    public const string FaildToGetUsedPackingUnit = "FaildToGetUsedPackingUnit";
    public const string FailedToAddStockBalance = "Failed To Add Stock Balance";
    public const string FinancialPeriodIsUsedInEntries = "FinancialPeriodIsUsedInEntries";
    public const string FinancialPeriodIsUsedInInventoryTransactions = "FinancialPeriodIsUsedInInventoryTransactions";
    public const string FinancialPeriodWithYearNumberIsExisted = "FinancialPeriodWithYearNumberIsExisted";
    public const string FinancialTransactionIsUsedInEntriesCannotUpdateYearName = "FinancialTransactionIsUsedInEntriesCannotUpdateYearName";
    public const string FinancialTransactionIsUsedInInventoryTransactionsCannotUpdateYearName = "FinancialTransactionIsUsedInInventoryTransactionsCannotUpdateYearName";
    public const string INVENTORY_THRESHOLD_BRANCH_REQUIRED_WHEN_SCOPE_BRANCH = "INVENTORY_THRESHOLD_BRANCH_REQUIRED_WHEN_SCOPE_BRANCH";
    public const string INVENTORY_THRESHOLD_MIN_MUST_BE_LESS_THAN_MAX = "INVENTORY_THRESHOLD_MIN_MUST_BE_LESS_THAN_MAX";
    public const string INVENTORY_THRESHOLD_RANGES_MUST_NOT_OVERLAP = "INVENTORY_THRESHOLD_RANGES_MUST_NOT_OVERLAP";
    public const string ITEM_EGS_CODE_IS_DUPLICATED = "ITEM_EGS_CODE_IS_DUPLICATED";
    public const string ITEM_GS1_CODE_IS_DUPLICATED = "ITEM_GS1_CODE_IS_DUPLICATED";
    public const string InsufficientStockBalance = "InsufficientStockBalance";
    public const string InvalidDomainItem = "InvalidDomainItem";
    public const string NotLastPeriod = "NotLastPeriod";
    public const string OPERATION_FAILD = "OPERATION_FAILD";
    public const string OneOfBarcodesAlreadyExists = "One of barcodes  already exists";
    public const string OneOfBarcodesIsAssociatedWithAnotherProduct = "One of barcodes is associated with another product";
    public const string PLEASE_PROVIDE_NON_DUPLICATE_BARCODES = "PLEASE_PROVIDE_NON_DUPLICATE_BARCODES";
    public const string PLEASE_PROVIDE_NON_DUPLICATE_MANUFACTURER_COMPANIES = "PLEASE_PROVIDE_NON_DUPLICATE_MANUFACTURER_COMPANIES";
    public const string PLEASE_PROVIDE_NON_DUPLICATE_SUPPLIERS = "PLEASE_PROVIDE_NON_DUPLICATE_SUPPLIERS";
    public const string PRODUCT_NOT_FOUND = "PRODUCT_NOT_FOUND";
    public const string ParentIdIsRequired = "ParentIdIsRequired";
    public const string PartyAccountNotFound = "PartyAccountNotFound";
    public const string ProductEGSAlreadyExists = "Product EGS already exists";
    public const string ProductGS1AlreadyExists = "Product GS1 already exists";
    public const string ProductCodeAlreadyExists = "Product code already exists";
    public const string SomeChartOfAcountsNotExisted = "SomeChartOfAcountsNotExisted";
    public const string ThereAreEntriesAfterNewEndDate = "ThereAreEntriesAfterNewEndDate";
    public const string ThereAreInventoryTrasnsactionsAfterNewEndDate = "ThereAreInventoryTrasnsactionsAfterNewEndDate";
    public const string TransactionIsNotExportTransaction = "TransactionIsNotExportTransaction";
    public const string TransactionIsNotImportTransaction = "TransactionIsNotImportTransaction";
    public const string TransactionNotFound = "TransactionNotFound";
    public const string TransferIsNotPending = "TransferIsNotPending";
    public const string TransferNotFound = "TransferNotFound";
    public const string VARIANT_CODE_IS_DUPLICATED = "VARIANT_CODE_IS_DUPLICATED";
    public const string VARIANT_EGS_CODE_IS_DUPLICATED = "VARIANT_EGS_CODE_IS_DUPLICATED";
    public const string VARIANT_GS1_CODE_IS_DUPLICATED = "VARIANT_GS1_CODE_IS_DUPLICATED";
    public const string VARIANT_ONE_OF_BARCODES_IS_DUPLICATED = "VARIANT_ONE_OF_BARCODES_IS_DUPLICATED";
    public const string VariantCombinationsAreOnlyAllowedForProductsOfTypeStock = "Variant combinations are only allowed for products of type Stock";
    public const string VariantNotFound = "VariantNotFound";
    public const string VariantWithNowStockBalance = "VariantWithNowStockBalance";

    // Identity
    public const string UserNotFound = "USER_NOT_FOUND";
    public const string IncorrectEmailOrPassword = "INCORRENCT_EMAIL_OR_PASSWORD";
    public const string LoginFailed = "Login failed.";
    public const string UsernameAlreadyExists = "UsernameAlreadyExists";
    public const string UserCreationFailed = "UserCreationFailed";
    public const string InvalidAccessTokenOrRefreshToken = "INVALID_ACCESS_TOKEN_OR_REFRESH_TOKEN";
    public const string InvalidRefreshToken = "INVALID_REFRESH_TOKEN";

    // Platform
    public const string AuditLogNotFound = "AuditLogNotFound";
}

/// <summary>
/// Centralized success message keys
/// </summary>
public static class SuccessMessages
{
    public const string CreatedSuccessfully = "CreatedSuccessfully";
    public const string UpdatedSuccessfully = "UpdatedSuccessfully";
    public const string DeletedSuccessfully = "DeletedSuccessfully";
    public const string BulkCreatedSuccessfully = "BulkCreatedSuccessfully";
    public const string BulkUpdatedSuccessfully = "BulkUpdatedSuccessfully";
    public const string BulkDeletedSuccessfully = "BulkDeletedSuccessfully";
    public const string OperationSuccessful = "OperationSuccessful";
    public const string AddedSuccessfullyWithName = "AddedSuccessfullyWithName";
    public const string DeletedSuccessfullyWithName = "DeletedSuccessfullyWithName";
    public const string UpdatedSuccessfullyWithName = "UpdatedSuccessfullyWithName";
}

