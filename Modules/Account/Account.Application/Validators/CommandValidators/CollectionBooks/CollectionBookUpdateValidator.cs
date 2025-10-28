using Account.Domain.Commands.CollectionBooks;
using Shared.Application.Validators.BaseCommandValidators.UpdateCommandValidators;
using Account.Domain.Models.Entities.CollectionBooks;

namespace Account.Application.Validators.CommandValidators.CollectionBooks;

public class CollectionBookUpdateValidator : BaseSettingUpdateValidator<CollectionBookUpdateCommand, CollectionBook>
{ }