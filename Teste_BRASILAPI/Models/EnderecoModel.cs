using System.Text.Json.Serialization;

namespace Teste_BRASILAPI.Models
{
    public class EnderecoModel
    {
        [JsonPropertyName("cep")]
        public string? Cep { get; set; }

        [JsonPropertyName("state")]
        public string? Estado { get; set; }

        [JsonPropertyName("city")]
        public string? Cidade { get; set; }

        [JsonPropertyName("neighborhood")]
        public string? Região { get; set; }

        [JsonPropertyName("street")]
        public string? Rua { get; set; }

        [JsonPropertyName("service")]
        public string? Servico { get; set; }

        public bool Verificacao { get; set; }

        public string? ErrorMessage { get; set; }


    }
}

