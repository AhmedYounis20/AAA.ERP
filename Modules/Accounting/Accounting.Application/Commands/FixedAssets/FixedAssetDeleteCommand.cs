using Accounting.Domain.Models.Entities.SubLeadgers;
using ERP.BuildingBlocks.Application.Responses;
using Shared;

namespace Accounting.Application.Commands.FixedAssets;

public record FixedAssetDeleteCommand(Guid Id) : ICommand<ApiResponse<FixedAsset>>;
