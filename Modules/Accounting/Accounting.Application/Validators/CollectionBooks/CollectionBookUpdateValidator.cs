using Accounting.Application.Commands.CollectionBooks;
using ERP.BuildingBlocks.Application.Validators.Account.ComandValidators.BaseCommandValidators.UpdateCommandValidators;
using Accounting.Domain.Models.Entities.CollectionBooks;

namespace Accounting.Application.Validators.CollectionBooks;

public class CollectionBookUpdateValidator : BaseSettingUpdateValidator<CollectionBookUpdateCommand, CollectionBook>;
