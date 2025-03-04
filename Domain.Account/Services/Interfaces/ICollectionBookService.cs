using Domain.Account.Commands.AccountGuides;
using Domain.Account.Models.Entities.AccountGuide;
using Domain.Account.Models.Entities.CollectionBooks;
using Domain.Account.Services.BaseServices.interfaces;
using Microsoft.VisualBasic;

namespace Domain.Account.Services.Interfaces;

public interface ICollectionBookService: IBaseSettingService<CollectionBook,CollectionBookCreateCommand,CollectionBookUpdateCommand>{}