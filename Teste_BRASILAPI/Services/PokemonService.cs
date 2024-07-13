using System.Text.Json;
using System.Text.Json.Serialization;
using Teste_BRASILAPI.Interfaces;
using Teste_BRASILAPI.Models;

namespace Teste_BRASILAPI.Services;

public class PokemonService : IPokemon
{
    public async Task<List<PokemonListItem>> GetAllPokemon()
    {
        var request = new HttpRequestMessage(HttpMethod.Get, "https://pokeapi.co/api/v2/pokemon?limit=1025");

        using (var client = new HttpClient())
        {
            var response = await client.SendAsync(request);
            var content = await response.Content.ReadAsStringAsync();
            var pokemonListResponse = JsonSerializer.Deserialize<PokemonListResponse>(content);

            // Adiciona a URL da imagem e o ID a cada Pokémon
            foreach (var pokemon in pokemonListResponse.Results)
            {
                pokemon.Id = ExtractIdFromUrl(pokemon.Url);
                pokemon.ImageUrl = GetPokemonImageUrl(pokemon.Id);
            }

            return pokemonListResponse.Results;
        }
    }

    #region BuscarPokemon
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

				// Busca informações de evolução
				await AdicionarEvolucoes(client, objResponse);
				// Busca informações de variantes
				await AdicionarVariantes(client, objResponse);
				objResponse.IconUrl = GetPokemonIconUrl(objResponse.Id);

				return objResponse;
			}
			else
			{
				objResponse.ErrorMessage = "Pókemon não encontrado! Por favor digite um Pokémon válido.";
				return objResponse;
			}
		}
	}
	#endregion

	#region Evoluções e Variantes
	private async Task AdicionarEvolucoes(HttpClient client, PokemonModel pokemon)
	{
		try
		{
			var response = await client.GetAsync($"https://pokeapi.co/api/v2/pokemon-species/{pokemon.Id}");
			var content = await response.Content.ReadAsStringAsync();
			var species = JsonSerializer.Deserialize<SpeciesModel>(content);

			var evolutionChainUrl = species.EvolutionChain.Url;
			response = await client.GetAsync(evolutionChainUrl);
			content = await response.Content.ReadAsStringAsync();
			var evolutionChain = JsonSerializer.Deserialize<EvolutionChainModel>(content);

			var evolutions = new List<Evolution>();
			AddEvolutions(evolutionChain.Chain, evolutions, pokemon.Nome.ToLower());

			pokemon.Evolutions = evolutions;
		}
		catch (Exception ex)
		{
			pokemon.ErrorMessage = "Erro ao adicionar evoluções: " + ex.Message;
		}
	}

	private void AddEvolutions(ChainLink chain, List<Evolution> evolutions, string currentPokemonName)
	{
		try
		{
			if (chain.Species.Name != currentPokemonName)
			{
				var id = ExtractIdFromUrl(chain.Species.Url);

				var evolution = new Evolution
				{
					Name = chain.Species.Name,
					ImageUrl = GetPokemonImageUrl(id)
				};
				evolutions.Add(evolution);
			}

			foreach (var evolvesTo in chain.EvolvesTo)
			{
				AddEvolutions(evolvesTo, evolutions, currentPokemonName);
			}
		}
		catch (Exception ex)
		{
			Console.WriteLine("Erro ao adicionar evolução: " + ex.Message);
		}
	}

	private int ExtractIdFromUrl(string url)
	{
		try
		{
			var id = url.Split(new[] { '/' }, StringSplitOptions.RemoveEmptyEntries).Last();
			return int.Parse(id);
		}
		catch (Exception ex)
		{
			Console.WriteLine("Erro ao extrair ID da URL: " + ex.Message);
			throw;
		}
	}

	private async Task AdicionarVariantes(HttpClient client, PokemonModel pokemon)
	{
		try
		{
			var response = await client.GetAsync($"https://pokeapi.co/api/v2/pokemon-species/{pokemon.Id}");
			var content = await response.Content.ReadAsStringAsync();
			var species = JsonSerializer.Deserialize<SpeciesModel>(content);

			var variants = new List<Variant>();
			foreach (var variety in species.Varieties)
			{
				if (variety.Pokemon.Name != pokemon.Nome.ToLower())
				{
					var variantId = await GetPokemonId(client, variety.Pokemon.Name);
					variants.Add(new Variant { Name = variety.Pokemon.Name, ImageUrl = GetPokemonImageUrl(variantId) });
				}
			}

			pokemon.Variants = variants;
		}
		catch (Exception ex)
		{
			pokemon.ErrorMessage = "Erro ao adicionar variantes: " + ex.Message;
		}
	}

	private async Task<int> GetPokemonId(HttpClient client, string name)
	{
		try
		{
			var response = await client.GetAsync($"https://pokeapi.co/api/v2/pokemon/{name}");
			var content = await response.Content.ReadAsStringAsync();
			var pokemon = JsonSerializer.Deserialize<PokemonModel>(content);
			return pokemon.Id;
		}
		catch (Exception ex)
		{
			Console.WriteLine("Erro ao obter ID do Pokémon: " + ex.Message);
			throw;
		}
	}

	private string GetPokemonImageUrl(int id)
	{
		try
		{
			return $"https://raw.githubusercontent.com/PokeAPI/sprites/master/sprites/pokemon/other/official-artwork/{id}.png";
		}
		catch (Exception ex)
		{
			Console.WriteLine("Erro ao obter URL da imagem do Pokémon: " + ex.Message);
			throw;
		}
	}

	private string GetPokemonIconUrl(int pokemonId)
	{
		try
		{
			return $"https://raw.githubusercontent.com/PokeAPI/sprites/master/sprites/pokemon/{pokemonId}.png";
		}
		catch (Exception ex)
		{
			Console.WriteLine("Erro ao obter URL do ícone do Pokémon: " + ex.Message);
			throw;
		}
	}
	#endregion
}