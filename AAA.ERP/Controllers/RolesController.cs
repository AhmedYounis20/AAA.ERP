using AAA.ERP.Controllers.BaseControllers;
using AutoMapper;
using Domain.Account.Commands.AccountGuides;
using Domain.Account.Commands.Roles;
using Domain.Account.InputModels;
using Domain.Account.Models.Entities.AccountGuide;
using Domain.Account.Models.Entities.Roles;
using Domain.Account.Services.Interfaces;
using Domain.Account.Validators.InputValidators;
using MediatR;
using Microsoft.Extensions.Localization;
using Shared.Resources;

namespace AAA.ERP.Controllers;

[Route("api/[controller]")]
[ApiController]
public class RolesController : BaseSettingController<Role, RoleCreateCommand,RoleUpdateCommand>
{
    public RolesController(IRoleService service,
        IStringLocalizer<Resource> localizer,
        ISender mapper) : base(service, localizer, mapper)
    { }

    [HttpPost]
    public virtual async Task<IActionResult> Create([FromBody] RoleCreateCommand input)
    {
        return await CreateRecord(input);
    }
    [HttpGet]
    public virtual async Task<IActionResult> Get()
    {
        return await GetAllRecords();
    }
    [HttpGet("{id}")]
    public virtual async Task<IActionResult> Get(Guid id)
    {
        return await GetRecord(id);
    }
    [HttpPut("{id}")]
    public virtual async Task<IActionResult> Update(Guid id, [FromBody] RoleUpdateCommand input)
    {
        return await UpdateRecord(id, input);
    }
    [HttpDelete("{id}")]
    public virtual async Task<IActionResult> DeleteAsync(Guid id)
    {
        return await DeleteRecord(id);
    }

}