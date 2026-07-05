using Accounting.Application.Commands.CollectionBooks;
using ERP.Application.Validators.Account.ComandValidators.BaseCommandValidators.CreateCommandValidators;
using ERP.Domain.Models.Entities.Account.CollectionBooks;

namespace Accounting.Application.Validators.CollectionBooks;

public class CollectionBookCreateValidator : BaseSettingCreateValidator<CollectionBookCreateCommand, CollectionBook>;
