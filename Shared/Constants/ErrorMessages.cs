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
}

