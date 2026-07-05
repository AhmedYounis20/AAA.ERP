using Accounting.Application.Commands.CollectionBooks;
using ERP.Application.Validators.Account.ComandValidators.BaseCommandValidators.CreateCommandValidators;
using Accounting.Domain.Models.Entities.CollectionBooks;

namespace Accounting.Application.Validators.CollectionBooks;

public class CollectionBookCreateValidator : BaseSettingCreateValidator<CollectionBookCreateCommand, CollectionBook>;
