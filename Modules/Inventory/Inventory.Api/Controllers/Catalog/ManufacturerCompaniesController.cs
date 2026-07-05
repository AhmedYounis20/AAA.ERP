using Inventory.Application.Commands.ManufacturerCompanies;
using Inventory.Application.Services;
using Inventory.Domain.Models.Entities.ManufacturerCompanies;
using ERP.BuildingBlocks.Api.Controllers;
using ERP.BuildingBlocks.Application.Responses;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Inventory.Api.Controllers.Catalog;

/// <summary>CRUD API for manufacturer company settings (hybrid CQRS).</summary>
[Route("api/[controller]")]
[ApiController]
[Authorize]
public class ManufacturerCompaniesController(ISender sender, IManufacturerCompanyQueries queries)
    : HybridSettingController<ManufacturerCompany, ManufacturerCompanyCreateCommand, ManufacturerCompanyUpdateCommand>(sender, queries)
{
    protected override IRequest<ApiResponse<ManufacturerCompany>> BuildDeleteCommand(Guid id)
        => new ManufacturerCompanyDeleteCommand(id);
}
