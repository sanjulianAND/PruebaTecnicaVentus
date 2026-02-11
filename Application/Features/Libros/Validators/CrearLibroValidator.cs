using Application.Features.Libros.Create;
using FluentValidation;

namespace Application.Features.Libros.Validators;

public class CrearLibroValidator : AbstractValidator<CrearLibroCommand>
{
    public CrearLibroValidator()
    {
        RuleFor(x => x.Datos.Titulo)
            .NotEmpty().WithMessage("El título es obligatorio")
            .MaximumLength(300).WithMessage("El título no puede exceder 300 caracteres");

        RuleFor(x => x.Datos.Anio)
            .GreaterThan(0).WithMessage("El año debe ser mayor a 0")
            .LessThanOrEqualTo(DateTime.Now.Year).WithMessage("El año no puede ser futuro");

        RuleFor(x => x.Datos.Genero)
            .NotEmpty().WithMessage("El género es obligatorio")
            .MaximumLength(100).WithMessage("El género no puede exceder 100 caracteres");

        RuleFor(x => x.Datos.NumeroPaginas)
            .GreaterThan(0).WithMessage("El número de páginas debe ser mayor a 0");

        RuleFor(x => x.Datos.AutorId)
            .GreaterThan(0).WithMessage("El autor es obligatorio");
    }
}
