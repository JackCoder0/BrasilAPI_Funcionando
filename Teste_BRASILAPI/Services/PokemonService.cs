using System.Text.Json;
using Teste_BRASILAPI.Interfaces;
using Teste_BRASILAPI.Models;

namespace Teste_BRASILAPI.Services;

public class PokemonService : IPokemon
{
    public async Task<PokemonModel> BuscarPokemon(string name)
    {
        var request = new HttpRequestMessage(HttpMethod.Get, $"https://pokeapi.co/api/v2/pokemon/{name}");

        using (var client = new HttpClient())
        {
            var responsePokeApi = await client.SendAsync(request);
            var contentResp = await responsePokeApi.Content.ReadAsStringAsync();
            var objResponse = JsonSerializer.Deserialize<PokemonModel>(contentResp);

            if (responsePokeApi.IsSuccessStatusCode)
            {
                objResponse.Verificacao = true;
                return objResponse;
            }
            else
            {
                objResponse.ErrorMessage = "Pókemon não encontrado! Por favor digite um Pokémon válido.";
                return objResponse;
            }
        }
    }
}
