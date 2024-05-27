using Teste_BRASILAPI.Models;

namespace Teste_BRASILAPI.Interfaces;

public interface IPokemon
{
    Task<PokemonModel> BuscarPokemon(string name);
}
