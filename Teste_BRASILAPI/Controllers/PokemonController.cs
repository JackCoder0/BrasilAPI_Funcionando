using Microsoft.AspNetCore.Mvc;
using Teste_BRASILAPI.Services;

namespace Teste_BRASILAPI.Controllers;

[ApiController]
[Route("{controller}")]
public class PokemonController : Controller
{
    private readonly PokemonService _pokemonService;

    public PokemonController(PokemonService pokemonService)
    {
        _pokemonService = pokemonService;
    }

    [HttpGet]
    public IActionResult Index()
    {
        return View();
    }

    [HttpGet("{action}")]
    public async Task<IActionResult> PokemonInfo(string name)
    {
        var response = await _pokemonService.BuscarPokemon(name);
        return View(response);
    }
}
