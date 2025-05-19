using Domain.Account.Commands.AccountGuides;
using Domain.Account.Models.Entities.AccountGuide;
using Domain.Account.Models.Entities.CollectionBooks;
using ERP.Application.Validators.Account.ComandValidators.BaseCommandValidators.UpdateCommandValidators;
using Shared.Responses;

namespace ERP.Application.Validators.Account.ComandValidators.CollectionBooks;

public class CollectionBookUpdateValidator : BaseSettingUpdateValidator<CollectionBookUpdateCommand, CollectionBook>
{ }