using Accounting.Application.Commands.CollectionBooks;
using ERP.BuildingBlocks.Application.Validators.Account.CommandValidators.BaseCommandValidators.CreateCommandValidators;
using Accounting.Domain.Models.Entities.CollectionBooks;

namespace Accounting.Application.Validators.CollectionBooks;

public class CollectionBookCreateValidator : BaseSettingCreateValidator<CollectionBookCreateCommand, CollectionBook>;
