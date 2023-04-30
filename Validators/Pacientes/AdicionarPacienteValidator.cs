using FluentValidation;
using System.Text.RegularExpressions;
using WebSisMed.Models.Contexts;
using WebSisMed.ViewModels.Pacientes;

namespace WebSisMed.Validators.Pacientes
{
    public class AdicionarPacienteValidator : AbstractValidator<AdicionarPacienteViewModel>
    {
        public AdicionarPacienteValidator(SisMedContext context)
        {
            RuleFor(x => x.CPF).NotEmpty().WithMessage("Campo obrigatório.")
                               .Must(cpf => Regex.Replace(cpf, "[^0-9]", "").Length == 11).WithMessage("O CPF deve ter até {MaxLength} caracteres.")
                               .Must(cpf => !context.Pacientes.Any(p => p.CPF == cpf)).WithMessage("Este CPF já está em uso.");

            RuleFor(x => x.Nome).NotEmpty().WithMessage("Campo obrigatório.")
                                .MaximumLength(150).WithMessage("O Nome deve ter até {MaxLength} caracteres.");

            RuleFor(x => x.DataNascimento).NotEmpty().WithMessage("Campo obrigatório.")
                                .Must(data => data <= DateTime.Today).WithMessage("A Data de nascimento não pode ser futura.");
        }
    }
}
