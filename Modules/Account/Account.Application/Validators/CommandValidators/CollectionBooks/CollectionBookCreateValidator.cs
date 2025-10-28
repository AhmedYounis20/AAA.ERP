using Account.Domain.Commands.CollectionBooks;
using Shared.Application.Validators.BaseCommandValidators.CreateCommandValidators;
using Account.Domain.Models.Entities.CollectionBooks;

namespace Account.Application.Validators.CommandValidators.CollectionBooks;

public class CollectionBookCreateValidator : BaseSettingCreateValidator<CollectionBookCreateCommand, CollectionBook>
{ }