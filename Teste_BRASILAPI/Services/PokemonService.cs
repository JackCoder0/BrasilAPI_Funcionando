using System.Text.Json;
using Teste_BRASILAPI.Interfaces;
using Teste_BRASILAPI.Models;

namespace Teste_BRASILAPI.Services;

public class PokemonService : IPokemon
{
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

	private void AddEvolutions(ChainLink chain, List<Evolution> evolutions, string currentPokemonName)
	{
		if (chain.Species.Name != currentPokemonName)
		{
			var evolution = new Evolution
			{
				Name = chain.Species.Name,
				ImageUrl = GetPokemonImageUrl(chain.Species.Url)
			};
			evolutions.Add(evolution);
		}

		foreach (var evolvesTo in chain.EvolvesTo)
		{
			AddEvolutions(evolvesTo, evolutions, currentPokemonName);
		}
	}

	private async Task AdicionarVariantes(HttpClient client, PokemonModel pokemon)
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
				variants.Add(new Variant { Name = variety.Pokemon.Name, ImageUrl = GetPokemonImageUrl(variantId.ToString()) });
			}
		}

		pokemon.Variants = variants;
	}

	private async Task<int> GetPokemonId(HttpClient client, string name)
	{
		var response = await client.GetAsync($"https://pokeapi.co/api/v2/pokemon/{name}");
		var content = await response.Content.ReadAsStringAsync();
		var pokemon = JsonSerializer.Deserialize<PokemonModel>(content);
		return pokemon.Id;
	}

	private string GetPokemonImageUrl(string speciesUrl)
	{
		var id = speciesUrl.Split(new[] { '/' }, StringSplitOptions.RemoveEmptyEntries).Last();
		return $"https://raw.githubusercontent.com/PokeAPI/sprites/master/sprites/pokemon/other/official-artwork/{id}.png";
	}

	private string GetPokemonIconUrl(int pokemonId)
	{
		string defaultSpriteUrl = $"https://raw.githubusercontent.com/PokeAPI/sprites/master/sprites/pokemon/{pokemonId}.png";
		string generationVIIIIconUrl = $"https://raw.githubusercontent.com/PokeAPI/sprites/master/sprites/pokemon/versions/generation-viii/icons/{pokemonId}.png";

		// Verifica se o ícone da geração VIII existe, se não, retorna a sprite padrão
		if (PokemonIconExists(generationVIIIIconUrl))
		{
			return generationVIIIIconUrl;
		}
		else
		{
			return defaultSpriteUrl;
		}
	}
	private bool PokemonIconExists(string url)
	{
		using (var client = new HttpClient())
		{
			var response = client.GetAsync(url).Result; // Síncrono para simplificar
			return response.IsSuccessStatusCode;
		}
	}
}
#endregion