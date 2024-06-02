using Microsoft.AspNetCore.Mvc;
using Teste_BRASILAPI.Models;
using Teste_BRASILAPI.Services;
using System.Threading.Tasks;
using System.Text.Json;

namespace Teste_BRASILAPI.Controllers;

[ApiController]
[Route("{controller}")]
public class PokemonController : Controller
{
	private readonly PokemonService _pokemonService;

	private readonly Dictionary<int, string> _tipoParaImagem = new Dictionary<int, string>
		{
			{ 1, "normal.png" },
			{ 2, "fighting.png" },
			{ 3, "flying.png" },
			{ 4, "poison.png" },
			{ 5, "ground.png" },
			{ 6, "rock.png" },
			{ 7, "bug.png" },
			{ 8, "ghost.png" },
			{ 9, "steel.png" },
			{ 10, "fire.png" },
			{ 11, "water.png" },
			{ 12, "grass.png" },
			{ 13, "electric.png" },
			{ 14, "psychic.png" },
			{ 15, "ice.png" },
			{ 16, "dragon.png" },
			{ 17, "dark.png" },
			{ 18, "fairy.png" }
		};

	public PokemonController(PokemonService pokemonService)
	{
		_pokemonService = pokemonService;
	}

	public IActionResult Index()
	{
		return View();
	}

	[HttpGet("action")]
	public async Task<IActionResult> PokemonInfo(string name)
	{
		PokemonModel pokemon;

		try
		{
			pokemon = await _pokemonService.BuscarPokemon(name);

			if (pokemon == null)
			{
				pokemon = new PokemonModel
				{
					Verificacao = false,
					ErrorMessage = "Pokémon não encontrado.",
					OfficialArtworkUrl = Url.Content("~/images/pokemon-not-found.png")
				};
			}
			else
			{
				pokemon.Verificacao = true;
			}
		}
		catch
		{
			pokemon = new PokemonModel
			{
				Verificacao = false,
				ErrorMessage = "Pokémon não encontrado.",
				OfficialArtworkUrl = Url.Content("~/images/pokemon-not-found.png")
			};
		}

		return View("PokemonInfo", pokemon);
	}
}