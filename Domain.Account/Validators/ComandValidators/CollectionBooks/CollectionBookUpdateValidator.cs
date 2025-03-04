using Domain.Account.Commands.AccountGuides;
using Domain.Account.Models.Entities.AccountGuide;
using Domain.Account.Models.Entities.CollectionBooks;
using Domain.Account.Validators.ComandValidators.BaseCommandValidators.UpdateCommandValidators;
using Shared.Responses;

namespace Domain.Account.Validators.ComandValidators.CollectionBooks;

public class CollectionBookUpdateValidator : BaseSettingUpdateValidator<CollectionBookUpdateCommand,CollectionBook>
{ }