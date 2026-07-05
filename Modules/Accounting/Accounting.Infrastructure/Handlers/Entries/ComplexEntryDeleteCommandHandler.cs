using Accounting.Application.Commands.Entries;
using Accounting.Application.Services;
using Accounting.Domain.Models.Entities.Entries;

namespace Accounting.Infrastructure.Handlers.Entries;

public class ComplexEntryDeleteCommandHandler(IComplexEntryService complexEntryService)
    : ICommandHandler<ComplexEntryDeleteCommand, ApiResponse<Entry>>
{
    public Task<ApiResponse<Entry>> Handle(ComplexEntryDeleteCommand request, CancellationToken cancellationToken)
        => complexEntryService.Delete(request.Id);
}
