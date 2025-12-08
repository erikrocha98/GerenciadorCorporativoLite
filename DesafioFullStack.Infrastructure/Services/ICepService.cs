using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DesafioFullStack.Infrastructure.Services
{
    public interface ICepService
    {
        Task<CepResponse?> BuscarCepAsync(string cep);
        Task<bool> ValidarCepAsync(string cep);
    }

    public class CepResponse
    {
        public string Cep { get; set; } = string.Empty;
        public string Logradouro { get; set; } = string.Empty;
        public string Bairro { get; set; } = string.Empty;
        public string Cidade { get; set; } = string.Empty;
        public string Uf { get; set; } = string.Empty;
    }
}
