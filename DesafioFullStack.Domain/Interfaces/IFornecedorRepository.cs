using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DesafioFullStack.Domain.Entities;


namespace DesafioFullStack.Domain.Interfaces
{
    public interface IFornecedorRepository : IRepository<Fornecedor>
    {
        Task<bool> CpfCnpjExistsAsync(string cpfCnpj, Guid? fornecedorIdToExclude = null);
        Task<IEnumerable<Fornecedor>> GetByFiltersAsync(string? nome = null, string? cpfCnpj = null);
        Task<IEnumerable<Empresa>> GetEmpresasByFornecedorIdAsync(Guid fornecedorId);
    }
}
