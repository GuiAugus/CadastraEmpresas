using System.Text.Json;

namespace CadastraEmpresas.Services
{
    public class ReceitaWSService
    {
        private readonly HttpClient _httpClient;
        private const string BaseUrl = "https://www.receitaws.com.br/v1/cnpj/";

        public ReceitaWSService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<EmpresaResponse?> ConsultarCnpj(string cnpj)
        {
            var url = $"{BaseUrl}{cnpj}";
            var response = await _httpClient.GetAsync(url);

            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                var EmpresaResponse = JsonSerializer.Deserialize<EmpresaResponse>(content, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });
                return EmpresaResponse;
            }

            return null;

        }
    }
    
    public class EmpresaResponse
    {
        public string? Nome { get; set; } 
        public string? Fantasia { get; set; } 
        public string? Cnpj { get; set; } 
        public string? Situacao { get; set; } 
        public string? Abertura { get; set; }
        public string? Tipo { get; set; }
        public string? Natureza_juridica { get; set; } 
        public string? Atividade_principal { get; set; } 
        
        public EnderecoResponse? Endereco { get; set; }

        public class EnderecoResponse
        {
            public string? Logradouro { get; set; }
            public string? Numero { get; set; }
            public string? Complemento { get; set; }
            public string? Bairro { get; set; }
            public string? Municipio { get; set; }
            public string? Uf { get; set; }
            public string? Cep { get; set; }
        }
    }
}