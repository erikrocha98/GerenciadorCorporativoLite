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
    public class EmpresaRepository : Repository<Empresa> , IEmpresaRepository
    {
        public EmpresaRepository(ApplicationDbContext context) : base(context)
        {
        }

        public async Task<bool> CnpjExistsAsync(string cnpj, Guid? empresaIdToExclude = null)
        {
            var query = _dbSet.Where(e => e.Cnpj == cnpj);

            if (empresaIdToExclude.HasValue)
                query = query.Where(e => e.Id != empresaIdToExclude.Value);

            return await query.AnyAsync();
        }

        public async Task<IEnumerable<Fornecedor>> GetFornecedoresByEmpresaIdAsync(Guid empresaId)
        {
            var empresa = await _dbSet
                .Include(e => e.EmpresaFornecedores)
                    .ThenInclude(ef => ef.Fornecedor)
                .FirstOrDefaultAsync(e => e.Id == empresaId);

            return empresa?.EmpresaFornecedores
                .Select(ef => ef.Fornecedor)
                .ToList() ?? new List<Fornecedor>();
        }

        public async Task VincularFornecedorAsync(Guid empresaId, Guid fornecedorId)
        {
            var vinculo = new EmpresaFornecedor
            {
                EmpresaId = empresaId,
                FornecedorId = fornecedorId,
                DataVinculo = DateTime.Now
            };

            _context.EmpresaFornecedores.Add(vinculo);
            await _context.SaveChangesAsync();
        }

        public async Task DesvincularFornecedorAsync(Guid empresaId, Guid fornecedorId)
        {
            var vinculo = await _context.EmpresaFornecedores
                .FirstOrDefaultAsync(ef => ef.EmpresaId == empresaId && ef.FornecedorId == fornecedorId);

            if (vinculo != null)
            {
                _context.EmpresaFornecedores.Remove(vinculo);
                await _context.SaveChangesAsync();
            }
        }
    }
}
