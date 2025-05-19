using ERP.Application.Validators.Account.ComandValidators.BaseCommandValidators.UpdateCommandValidators;
using ERP.Domain.Commands.Account.CollectionBooks;
using ERP.Domain.Models.Entities.Account.CollectionBooks;

namespace ERP.Application.Validators.Account.ComandValidators.CollectionBooks;

public class CollectionBookUpdateValidator : BaseSettingUpdateValidator<CollectionBookUpdateCommand, CollectionBook>
{ }