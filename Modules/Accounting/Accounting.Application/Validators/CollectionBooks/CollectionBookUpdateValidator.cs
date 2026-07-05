using Accounting.Application.Commands.CollectionBooks;
using ERP.Application.Validators.Account.ComandValidators.BaseCommandValidators.UpdateCommandValidators;
using ERP.Domain.Models.Entities.Account.CollectionBooks;

namespace Accounting.Application.Validators.CollectionBooks;

public class CollectionBookUpdateValidator : BaseSettingUpdateValidator<CollectionBookUpdateCommand, CollectionBook>;
