using Infrastructure.Abstractions;
using MediatR;

namespace Application.Features.Autores.Delete;

public record EliminarAutorCommand(int Id) : IRequest<bool>;

public class EliminarAutorHandler : IRequestHandler<EliminarAutorCommand, bool>
{
    private readonly IAutorRepository _autorRepository;

    public EliminarAutorHandler(IAutorRepository autorRepository)
    {
        _autorRepository = autorRepository;
    }

    public async Task<bool> Handle(EliminarAutorCommand request, CancellationToken cancellationToken)
    {
        if (!await _autorRepository.ExistsAsync(request.Id, cancellationToken))
            return false;

        await _autorRepository.DeleteAsync(request.Id, cancellationToken);
        return true;
    }
}
