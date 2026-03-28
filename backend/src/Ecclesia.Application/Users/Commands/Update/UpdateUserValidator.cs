using FluentValidation;

namespace Ecclesia.Application.Users.Commands.UpdateUser;

public class UpdateUserValidator : AbstractValidator<UpdateUserCommand>
{
    public UpdateUserValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty().WithMessage("El Id del usuario es requerido.")
            .NotEqual(Guid.Empty).WithMessage("El Id del usuario no es válido.");

        RuleFor(x => x.userDto.Name)
            .NotEmpty().WithMessage("El nombre es requerido.")
            .MaximumLength(100).WithMessage("El nombre no puede superar 100 caracteres.");

        RuleFor(x => x.userDto.Email)
            .NotEmpty().WithMessage("El email es requerido.")
            .EmailAddress().WithMessage("El email no tiene un formato válido.")
            .MaximumLength(256).WithMessage("El email no puede superar 256 caracteres.");
    }
}