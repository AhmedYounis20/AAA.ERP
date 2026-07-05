using ERP.BuildingBlocks.Application.Responses;
using Inventory.Domain.Models.Entities.ManufacturerCompanies;
using Shared;

namespace Inventory.Application.Commands.ManufacturerCompanies;

/// <summary>Deletes a manufacturer company record.</summary>
public record ManufacturerCompanyDeleteCommand(Guid Id) : ICommand<ApiResponse<ManufacturerCompany>>;
