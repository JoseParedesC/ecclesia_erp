using FluentValidation;

namespace Ecclesia.Application.Users.Commands.DeleteUser;

public class DeleteUserValidator : AbstractValidator<DeleteUserCommand>
{
    public DeleteUserValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty().WithMessage("El Id del usuario es requerido.")
            .NotEqual(Guid.Empty).WithMessage("El Id del usuario no es válido.");
    }
}