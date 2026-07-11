namespace ERP.BuildingBlocks.Application.Constants;

/// <summary>
/// FluentValidation message keys for localization (Accounting + Inventory validators).
/// </summary>
public static class ValidationMessages
{
    public const string AmountIsRequired = "AmountIsRequired";
    public const string BranchIdIsRequired = "BranchIdIsRequired";
    public const string ChartOfAccountAccountGuideRequired = "ChartOfAccountAccountGuideRequired";
    public const string ChartOfAccountCodeMaxLengthValidation = "ChartOfAccountCodeMaxLengthValidation";
    public const string ChartOfAccountCodeRequired = "ChartOfAccountCodeRequired";
    public const string ChartOfAccountIdIsRequired = "ChartOfAccountIdIsRequired";
    public const string ChartOfAccountNotValidAccountNature = "ChartOfAccountNotValidAccountNature";
    public const string ChartOfAccountMustBeDomainNode = "ChartOfAccountMustBeDomainNode";
    public const string CostCenterIsRequired = "CostCenterIsRequired";
    public const string CurrencyActiveOnDefault = "CurrencyActiveOnDefault";
    public const string CurrencyIdIsRequired = "CurrencyIdIsRequired";
    public const string CurrencySymbolIsRequired = "CurrencySymbolIsRequired";
    public const string CurrencySymbolMaxLength = "CurrencySymbolMaxLength";
    public const string DecimalDigitsMaxValue = "DecimalDigitsMAXValue";
    public const string DecimalDigitsMinValue = "DecimalDigitsMINValue";
    public const string DocumentNumberMaximumLength = "DocumentNumberMaximumLength";
    public const string EntryFinancialTransactionsRequired = "EntryFinancialTransactionsRequired";
    public const string ExchangeRateGreaterThanZero = "ExchangeRateGreaterThanZero";
    public const string ExchangeRateIsRequired = "ExchangeRateIsRequired";
    public const string ExchangeRateOnDefaultCurrency = "ExchangeRateOnDefaultCurrency";
    public const string FinancialPeriodIdIsRequired = "FinancialPeriodIdIsRequired";
    public const string FinancialPeriodMaximumLength = "FinancialPeriodMaximumLength";
    public const string FinancialPeriodRequiredYearNumber = "FinancialPeriodRequiredYearNumber";
    public const string FinancialPeriodStartDateRequired = "FinancialPeriodStartDateRequired";
    public const string MaxLength300 = "MaxLength300";
    public const string MonthDaysMinValue = "MonthDaysMINValue";
    public const string NameIsRequired = "NameIsRequired";
    public const string NameMaxLength = "NameMaxLength";
    public const string NameSecondLanguageIsRequired = "NameSecondLanguageIsRequired";
    public const string NameSecondLanguageMaxLength = "NameSecondLanguageMaxLength";
    public const string NotesMaximumLength = "NotesMaximumLength";
    public const string NotValidCustomerType = "NotValidCustomerType";
    public const string NotValidPeriodType = "NotValidPeriodType";
    public const string PleaseChooseValidNodeType = "PleaseChooseValidNodeType";
    public const string ReceiverNameMaximumLength = "ReceiverNameMaximumLength";
    // Inventory — FluentValidation message keys
    public const string AtLeastOneItemIsRequired = "At least one item is required";
    public const string AverageCostIsRequired = "AverageCostIsRequired";
    public const string BranchIsRequired = "Branch is required";
    public const string CodeIsRequired = "CodeIsRequired";
    public const string CurrentBalanceMustBeGreaterThanOrEqualToZero = "CurrentBalanceMustBeGreaterThanOrEqualToZero";
    public const string DiscountCannotBeNegative = "DiscountCannotBeNegative";
    public const string DiscountTypeIsNotValid = "DiscountTypeIsNotValid";
    public const string DocumentNumberMaxLength = "DocumentNumberMaxLength";
    public const string IdIsRequired = "IdIsRequired";
    public const string InvalidTransactionType = "InvalidTransactionType";
    public const string ItemIdIsRequired = "ItemIdIsRequired";
    public const string ItemIsRequired = "Item is required";
    public const string ItemsRequired = "ItemsRequired";
    public const string LastCostPriceIsRequired = "LastCostPriceIsRequired";
    public const string MaximumBalanceMustBeGreaterThanOrEqualToZero = "MaximumBalanceMustBeGreaterThanOrEqualToZero";
    public const string MinimumBalanceMustBeGreaterThanOrEqualToZero = "MinimumBalanceMustBeGreaterThanOrEqualToZero";
    public const string NotesMaxLength = "NotesMaxLength";
    public const string ONLY_ONE_IS_NEEDED_EGS_OR_GS1 = "ONLY_ONE_IS_NEEDED_EGS_OR_GS1";
    public const string PackingUnitIdIsRequired = "PackingUnitIdIsRequired";
    public const string PackingUnitIsRequired = "Packing unit is required";
    public const string PackingUnitIsRequired2 = "PackingUnitIsRequired";
    public const string PackingUnitSellingPriceIsRequired = "PackingUnitSellingPriceIsRequired";
    public const string PackingUnitsIsRequired = "PackingUnitsIsRequired";
    public const string PartsCountIsRequired = "PartsCountIsRequired";
    public const string PercentCannotBeNegative = "PercentCannotBeNegative";
    public const string PercentCannotExceed100 = "PercentCannotExceed100";
    public const string PriceAmountIsRequired = "PriceAmountIsRequired";
    public const string QuantityMustBeGreaterThanZero = "Quantity must be greater than zero";
    public const string QuantityMustBeGreaterThanZero2 = "QuantityMustBeGreaterThanZero";
    public const string SellingPriceIsRequired = "SellingPriceIsRequired";
    public const string TotalCostMustBeGreaterThanOrEqualToZero = "Total cost must be greater than or equal to zero";
    public const string TotalCostMustBeGreaterThanOrEqualToZero2 = "TotalCostMustBeGreaterThanOrEqualToZero";
    public const string TransactionDateIsRequired = "Transaction date is required";
    public const string TransactionDateIsRequired2 = "TransactionDateIsRequired";
    public const string TransactionIDIsRequired = "Transaction ID is required";
    public const string TransactionPartyIdIsRequired = "TransactionPartyIdIsRequired";
    public const string TransactionPartyIsRequired = "Transaction party is required";
    public const string UnitCostMustBeGreaterThanOrEqualToZero = "UnitCostMustBeGreaterThanOrEqualToZero";
    public const string VariantIdIsRequired = "VariantIdIsRequired";
}
