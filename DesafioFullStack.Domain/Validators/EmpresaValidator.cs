using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DesafioFullStack.Domain.Entities;
using DesafioFullStack.Domain.ValueObjects;
using FluentValidation;

namespace DesafioFullStack.Domain.Validators
{
    public class EmpresaValidator: AbstractValidator<Empresa>
    {
        public EmpresaValidator() { 
            RuleFor(e => e.Cnpj)
                .NotEmpty().WithMessage("O CNPJ é obrigatório.")
                .Must(Cnpj.Validar).WithMessage("O CNPJ informado é inválido.")
                .Length(14).WithMessage("O CNPJ deve conter 14 caracteres.");

            RuleFor(e => e.NomeFantasia)
                .NotEmpty().WithMessage("O nome fantasia é obrigatório.")
                .MaximumLength(200).WithMessage("O nome fantasia deve conter no máximo 200 caracteres.");

            RuleFor(e => e.Cep)
                .NotEmpty().WithMessage("O CEP é obrigatório.")
                .Length(8).WithMessage("O CEP deve conter 8 caracteres.")
                .Must(Cep.Validar).WithMessage("O CEP informado é inválido.");

        }
    }
}
