using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DesafioFullStack.Domain.Entities
{
    public class Empresa
    {
        public Guid Id { get; set; }
        public string Cnpj { get; set; } = string.Empty;
        public string NomeFantasia { get; set; } = string.Empty;
        public string Cep { get; set; } = string.Empty;
        public DateTime DataCadastro { get; set; }
        public DateTime? DataAtualizacao { get; set; }

        // Relacionamento Many-to-Many
        public ICollection<EmpresaFornecedor> EmpresaFornecedores { get; set; } = new List<EmpresaFornecedor>();
    }
}
