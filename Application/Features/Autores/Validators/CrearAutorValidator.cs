using Application.DTOs;
using Application.Features.Autores.Create;
using FluentValidation;

namespace Application.Features.Autores.Validators;

public class CrearAutorValidator : AbstractValidator<CrearAutorCommand>
{
    public CrearAutorValidator()
    {
        RuleFor(x => x.Datos.NombreCompleto)
            .NotEmpty().WithMessage("El nombre completo es obligatorio")
            .MaximumLength(200).WithMessage("El nombre no puede exceder 200 caracteres");

        RuleFor(x => x.Datos.FechaNacimiento)
            .NotEmpty().WithMessage("La fecha de nacimiento es obligatoria")
            .LessThan(DateTime.Now).WithMessage("La fecha de nacimiento debe ser en el pasado");

        RuleFor(x => x.Datos.CiudadProcedencia)
            .NotEmpty().WithMessage("La ciudad de procedencia es obligatoria")
            .MaximumLength(100).WithMessage("La ciudad no puede exceder 100 caracteres");

        RuleFor(x => x.Datos.CorreoElectronico)
            .NotEmpty().WithMessage("El correo electrónico es obligatorio")
            .EmailAddress().WithMessage("El correo electrónico no es válido")
            .MaximumLength(150).WithMessage("El correo no puede exceder 150 caracteres");
    }
}
