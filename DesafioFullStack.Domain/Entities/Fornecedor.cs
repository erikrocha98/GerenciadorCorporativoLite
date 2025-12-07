using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DesafioFullStack.Domain.Entities
{
    public class Fornecedor
    {
        public Guid Id { get; set; }
        public string CpfCnpj { get; set; } = string.Empty;
        public string Nome { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Cep { get; set; } = string.Empty;

        // Campos específicos para Pessoa Física
        public string? Rg { get; set; }
        public DateTime? DataNascimento { get; set; }

        public bool EhPessoaFisica => CpfCnpj.Length == 11;
        public DateTime DataCadastro { get; set; }
        public DateTime? DataAtualizacao { get; set; }

        // Relacionamento Many-to-Many
        public ICollection<EmpresaFornecedor> EmpresaFornecedores { get; set; } = new List<EmpresaFornecedor>();
    }
}
