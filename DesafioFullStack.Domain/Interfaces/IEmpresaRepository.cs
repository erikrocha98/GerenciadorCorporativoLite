using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DesafioFullStack.Domain.Entities;


namespace DesafioFullStack.Domain.Interfaces
{
    public interface IEmpresaRepository: IRepository<Empresa>
    {
        Task<bool> CnpjExistsAsync(string cnpj, Guid? empresaIdToExclude=null);
        Task<bool> VinculoExisteAsync(Guid empresaId, Guid fornecedorId);
        Task<IEnumerable<Fornecedor>> GetFornecedoresByEmpresaIdAsync(Guid empresaId);
        Task VincularFornecedorAsync(Guid empresaId, Guid fornecedorId);
        Task DesvincularFornecedorAsync(Guid empresaId, Guid fornecedorId);
    }
}
