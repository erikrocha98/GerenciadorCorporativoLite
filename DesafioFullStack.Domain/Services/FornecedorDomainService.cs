using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DesafioFullStack.Domain.Entities;
using DesafioFullStack.Domain.Services;


namespace DesafioFullStack.Domain.Services
{
    public class FornecedorDomainService : IFornecedorDomainService
    {
        public Task<bool> PodeVincularFornecedorAsync(Fornecedor fornecedor, string cepEmpresa)
        {
            // Regra: Se empresa for do Paraná, não pode vincular fornecedor PF menor de idade
            var podeVincular = ValidarRegraParana(fornecedor, cepEmpresa);
            return Task.FromResult(podeVincular);
        }

        public bool ValidarRegraParana(Fornecedor fornecedor, string cepEmpresa)
        {
            // CEPs do Paraná começam com 80, 81, 82, 83, 84, 85, 86, 87
            var cepLimpo = new string(cepEmpresa.Where(char.IsDigit).ToArray());

            if (cepLimpo.Length != 8)
                return true; // CEP inválido, deixa outra validação cuidar

            var prefixoCep = int.Parse(cepLimpo.Substring(0, 2));
            var ehParana = prefixoCep >= 80 && prefixoCep <= 87;

            if (!ehParana)
                return true; // Não é Paraná, pode vincular

            // É Paraná, então verifica se é PF menor de idade
            if (!fornecedor.EhPessoaFisica)
                return true; // É PJ, pode vincular

            if (!fornecedor.DataNascimento.HasValue)
                return false; // PF sem data de nascimento, não pode

            var idade = CalcularIdade(fornecedor.DataNascimento.Value);
            return idade >= 18; // Só pode se for maior de idade
        }

        private int CalcularIdade(DateTime dataNascimento)
        {
            var hoje = DateTime.Today;
            var idade = hoje.Year - dataNascimento.Year;

            if (dataNascimento.Date > hoje.AddYears(-idade))
                idade--;

            return idade;
        }

    }
}
