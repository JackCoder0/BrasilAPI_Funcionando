using System.Dynamic;
using System.Text.Json;
using Teste_BRASILAPI.Interfaces;
using Teste_BRASILAPI.Models;

namespace Teste_BRASILAPI.Services
{
    public class EnderecoService : IEndereco
    {
        public async Task<EnderecoModel> BuscarEndereco(string cep)
        {
            var request = new HttpRequestMessage(HttpMethod.Get, $"https://brasilapi.com.br/api/cep/v1/{cep}");
            
            using (var client = new HttpClient())
            {
                var responseBrasilApi = await client.SendAsync(request);
                var contentResp = await responseBrasilApi.Content.ReadAsStringAsync();
                var objResponse = JsonSerializer.Deserialize<EnderecoModel>(contentResp);

                if (responseBrasilApi.IsSuccessStatusCode)
                {
                    objResponse.Verificacao = true;
                    return objResponse;
                }
                else
                {
                    objResponse.ErrorMessage = "Cep não encontrado! Por favor digite um CEP válido";
                    return objResponse;
                }
            }
        }
    }
}
