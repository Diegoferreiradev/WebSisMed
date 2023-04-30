using FluentValidation;
using WebSisMed.Models.Contexts;
using WebSisMed.ViewModels.Medicos;

namespace WebSisMed.Validators.Medicos
{
    public class EditarMedicoValidator : AbstractValidator<EditarMedicoViewModel>
    {
        public EditarMedicoValidator(SisMedContext context)
        {
            RuleFor(x => x.CRM).NotEmpty().WithMessage("Campo obrigatório.")
                               .MaximumLength(20).WithMessage("O CRM deve ter até {MaxLength} caracteres.");

            RuleFor(x => x.Nome).NotEmpty().WithMessage("Campo obrigatório.")
                                .MaximumLength(200).WithMessage("O CRM deve ter até {MaxLength} caracteres.");

            RuleFor(x => x).Must(x => !context.Medicos.Any(m => m.CRM == x.CRM && m.Id != x.Id)).WithMessage("Este CRM já está em uso.");
        }
    }
}
