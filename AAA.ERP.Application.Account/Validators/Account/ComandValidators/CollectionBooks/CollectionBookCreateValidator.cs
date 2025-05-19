using ERP.Application.Validators.Account.ComandValidators.BaseCommandValidators.CreateCommandValidators;
using ERP.Domain.Commands.Account.CollectionBooks;
using ERP.Domain.Models.Entities.Account.CollectionBooks;

namespace ERP.Application.Validators.Account.ComandValidators.CollectionBooks;

public class CollectionBookCreateValidator : BaseSettingCreateValidator<CollectionBookCreateCommand, CollectionBook>
{ }