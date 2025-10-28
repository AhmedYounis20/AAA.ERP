using MediatR;

namespace Shared.Domain.CQRS;

public interface ICommand : ICommand<Unit> { }

public interface ICommand <out TResponse> : IRequest<TResponse> { }
