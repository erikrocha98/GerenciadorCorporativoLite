using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.Json;


namespace DesafioFullStack.Infrastructure.Services
{
    public class CepService : ICepService
    {
        private readonly HttpClient _httpClient;

        public CepService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<CepResponse?> BuscarCepAsync(string cep)
        {
            var cepLimpo = new string(cep.Where(char.IsDigit).ToArray());

            if (cepLimpo.Length != 8)
                return null;

            try
            {
                // ViaCEP API
                var response = await _httpClient.GetAsync($"https://viacep.com.br/ws/{cepLimpo}/json/");

                if (!response.IsSuccessStatusCode)
                    return null;

                var content = await response.Content.ReadAsStringAsync();
                var cepData = JsonSerializer.Deserialize<ViaCepResponse>(content, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });

                // ViaCEP retorna {"erro": true} quando CEP não existe
                if (cepData == null || cepData.Erro)
                    return null;

                return new CepResponse
                {
                    Cep = cepData.Cep ?? cepLimpo,
                    Logradouro = cepData.Logradouro ?? string.Empty,
                    Bairro = cepData.Bairro ?? string.Empty,
                    Cidade = cepData.Localidade ?? string.Empty,
                    Uf = cepData.Uf ?? string.Empty
                };
            }
            catch
            {
                return null;
            }
        }

        public async Task<bool> ValidarCepAsync(string cep)
        {
            var resultado = await BuscarCepAsync(cep);
            return resultado != null;
        }

        // Classe auxiliar para deserializar a resposta da API ViaCEP
        private class ViaCepResponse
        {
            public string? Cep { get; set; }
            public string? Logradouro { get; set; }
            public string? Complemento { get; set; }
            public string? Bairro { get; set; }
            public string? Localidade { get; set; } // ViaCEP usa "localidade" para cidade
            public string? Uf { get; set; }
            public bool Erro { get; set; } // ViaCEP retorna este campo quando CEP não existe
        }
    }
}
