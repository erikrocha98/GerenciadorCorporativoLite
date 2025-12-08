using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DesafioFullStack.Domain.Entities;
using DesafioFullStack.Domain.Interfaces;
using DesafioFullStack.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace DesafioFullStack.Infrastructure.Repositories
{
    public class FornecedorRepository : Repository<Fornecedor>, IFornecedorRepository
    {
        public FornecedorRepository(ApplicationDbContext context) : base(context)
        {
        }

        public async Task<bool> CpfCnpjExistsAsync(string cpfCnpj, Guid? fornecedorIdToExclude = null)
        {
            var query = _dbSet.Where(f => f.CpfCnpj == cpfCnpj);

            if (fornecedorIdToExclude.HasValue)
                query = query.Where(f => f.Id != fornecedorIdToExclude.Value);

            return await query.AnyAsync();
        }

        public async Task<IEnumerable<Fornecedor>> GetByFiltersAsync(string? nome = null, string? cpfCnpj = null)
        {
            var query = _dbSet.AsQueryable();

            if (!string.IsNullOrWhiteSpace(nome))
                query = query.Where(f => f.Nome.Contains(nome));

            if (!string.IsNullOrWhiteSpace(cpfCnpj))
            {
                var cpfCnpjLimpo = new string(cpfCnpj.Where(char.IsDigit).ToArray());
                query = query.Where(f => f.CpfCnpj == cpfCnpjLimpo);
            }

            return await query.ToListAsync();
        }

        public async Task<IEnumerable<Empresa>> GetEmpresasByFornecedorIdAsync(Guid fornecedorId)
        {
            var fornecedor = await _dbSet
                .Include(f => f.EmpresaFornecedores)
                    .ThenInclude(ef => ef.Empresa)
                .FirstOrDefaultAsync(f => f.Id == fornecedorId);

            return fornecedor?.EmpresaFornecedores
                .Select(ef => ef.Empresa)
                .ToList() ?? new List<Empresa>();
        }
    }
}
