using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DesafioFullStack.Domain.Entities;


namespace DesafioFullStack.Domain.Services
{
    public interface IFornecedorDomainService
    {
        Task<bool> PodeVincularFornecedorAsync(Fornecedor fornecedor, string cepEmpresa);
        bool ValidarRegraParana(Fornecedor fornecedor, string cepEmpresa);
    }
}
