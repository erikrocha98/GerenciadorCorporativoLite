using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DesafioFullStack.Domain.Entities
{
    public class EmpresaFornecedor
    {
        public Guid EmpresaId { get; set; }
        public Empresa Empresa { get; set; } = null!;

        public Guid FornecedorId { get; set; }
        public Fornecedor Fornecedor { get; set; } = null!;

        public DateTime DataVinculo { get; set; }
    }
}
