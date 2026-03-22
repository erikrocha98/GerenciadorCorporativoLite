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
                // cep.la API
                var request = new HttpRequestMessage(HttpMethod.Get, $"https://cep.la/{cepLimpo}");
                request.Headers.Add("Accept", "application/json");

                var response = await _httpClient.SendAsync(request);

                if (!response.IsSuccessStatusCode)
                    return null;

                var content = await response.Content.ReadAsStringAsync();
                var cepData = JsonSerializer.Deserialize<CepLaResponse>(content, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });

                if (cepData == null || string.IsNullOrWhiteSpace(cepData.Cep))
                    return null;

                return new CepResponse
                {
                    Cep = cepData.Cep,
                    Logradouro = cepData.Logradouro ?? string.Empty,
                    Bairro = cepData.Bairro ?? string.Empty,
                    Cidade = cepData.Cidade ?? string.Empty,
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

        // Classe auxiliar para deserializar a resposta da API cep.la
        private class CepLaResponse
        {
            public string? Cep { get; set; }
            public string? Logradouro { get; set; }
            public string? Bairro { get; set; }
            public string? Cidade { get; set; } // cep.la usa "cidade"
            public string? Uf { get; set; }
        }
    }
}
