﻿using FluentValidation;
using System.Text.RegularExpressions;
using WebSisMed.Models.Contexts;
using WebSisMed.ViewModels.Pacientes;

namespace WebSisMed.Validators.Pacientes
{
    public class EditarPacienteValidator : AbstractValidator<EditarPacienteViewModel>
    {
        public EditarPacienteValidator(SisMedContext context)
        {
            RuleFor(x => x.CPF).NotEmpty().WithMessage("Campo obrigatório.")
                               .Must(cpf => Regex.Replace(cpf, "[^0-9]", "").Length == 11).WithMessage("O CPF deve ter até {MaxLength} caracteres.");
                               

            RuleFor(x => x.Nome).NotEmpty().WithMessage("Campo obrigatório.")
                                .MaximumLength(150).WithMessage("O nome deve ter até {MaxLength} caracteres.");

            RuleFor(x => x.DataNascimento).NotEmpty().WithMessage("Campo obrigatório.")
                                .Must(data => data <= DateTime.Today).WithMessage("A data de nascimento não pode ser futura.");

            RuleFor(x => x).Must(x => !context.Pacientes.Any(paciente => paciente.CPF == Regex.Replace(x.CPF, "[^0-9]", "") && paciente.Id != x.Id))
                           .WithMessage("Este CPF já está cadastrado.");
        }
    }
}
