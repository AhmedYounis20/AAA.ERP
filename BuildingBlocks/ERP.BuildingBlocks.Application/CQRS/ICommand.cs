using MediatR;

namespace Shared;

public interface ICommand : ICommand<Unit> { }

public interface ICommand <out TResponse> : IRequest<TResponse> { }

