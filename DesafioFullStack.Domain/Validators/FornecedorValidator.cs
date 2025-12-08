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
    public class FornecedorValidator : AbstractValidator<Fornecedor>
    {
        public FornecedorValidator()
        {
            RuleFor(f => f.CpfCnpj)
                .NotEmpty().WithMessage("CPF/CNPJ é obrigatório")
                .Must(BeValidCpfOrCnpj).WithMessage("CPF/CNPJ inválido");

            RuleFor(f => f.Nome)
                .NotEmpty().WithMessage("Nome é obrigatório")
                .MaximumLength(200).WithMessage("Nome deve ter no máximo 200 caracteres");

            RuleFor(f => f.Email)
                .NotEmpty().WithMessage("E-mail é obrigatório")
                .EmailAddress().WithMessage("E-mail inválido")
                .MaximumLength(200).WithMessage("E-mail deve ter no máximo 200 caracteres");

            RuleFor(f => f.Cep)
                .NotEmpty().WithMessage("CEP é obrigatório")
                .Length(8).WithMessage("CEP deve ter 8 dígitos")
                .Must(Cep.Validar).WithMessage("CEP inválido");

            // Validações específicas para Pessoa Física
            When(f => f.EhPessoaFisica, () =>
            {
                RuleFor(f => f.Rg)
                    .NotEmpty().WithMessage("RG é obrigatório para pessoa física")
                    .MaximumLength(20).WithMessage("RG deve ter no máximo 20 caracteres");

                RuleFor(f => f.DataNascimento)
                    .NotNull().WithMessage("Data de nascimento é obrigatória para pessoa física")
                    .LessThan(DateTime.Now).WithMessage("Data de nascimento deve ser no passado");
            });
        }

        private bool BeValidCpfOrCnpj(string cpfCnpj)
        {
            if (string.IsNullOrWhiteSpace(cpfCnpj))
                return false;

            var limpo = new string(cpfCnpj.Where(char.IsDigit).ToArray());

            return limpo.Length == 11 ? Cpf.Validar(limpo) : limpo.Length == 14 && Cnpj.Validar(limpo);
        }
    }
}
