using System.Text.Json.Serialization;

namespace Teste_BRASILAPI.Models;

public class PokemonModel
{
	[JsonPropertyName("name")]
	public string? Nome { get; set; }

	[JsonPropertyName("id")]
	public int Id { get; set; }

	[JsonPropertyName("height")]
	public long Altura { get; set; }

	[JsonPropertyName("weight")]
	public long Peso { get; set; }

	[JsonPropertyName("sprites")]
	public Sprites? Sprite { get; set; }

	[JsonPropertyName("types")]
	public List<TypeElement>? Tipos { get; set; }

	[JsonPropertyName("officialArtworkUrl")]
	public string? OfficialArtworkUrl { get; set; }
	public string? IconUrl { get; set; }
	public bool Verificacao { get; set; }
	public string? ErrorMessage { get; set; }
	public List<Evolution>? Evolutions { get; set; }
	public List<Variant>? Variants { get; set; }
	public string TipoPrincipal { get; internal set; }
}

//SPRITES DO PKM
public class Sprites
{
	[JsonPropertyName("other")]
	public OtherSprites? Other { get; set; }
}

public class OtherSprites
{
	[JsonPropertyName("official-artwork")]
	public OfficialArtwork? OfficialArtwork { get; set; }
}

public class OfficialArtwork
{
	[JsonPropertyName("front_default")]
	public string? FrontDefault { get; set; }
}

//TIPOS DO PKM
public class TypeElement
{
	[JsonPropertyName("slot")]
	public long Slot { get; set; }

	[JsonPropertyName("type")]
	public Type? Type { get; set; }
}

public class Type
{
	[JsonPropertyName("name")]
	public string? Name { get; set; }

	[JsonPropertyName("url")]
	public Uri? Url { get; set; }
}

public class Evolution
{
	public string? Name { get; set; }
	public string? ImageUrl { get; set; }
	public string? Id { get; set; }
}

public class Variant
{
	public string? Name { get; set; }
	public string? ImageUrl { get; set; }
	public string? Id { get; set; }
}

public class SpeciesModel
{
	[JsonPropertyName("evolution_chain")]
	public EvolutionChainUrl EvolutionChain { get; set; }

	[JsonPropertyName("varieties")]
	public List<Variety> Varieties { get; set; }
}

public class EvolutionChainUrl
{
	[JsonPropertyName("url")]
	public string Url { get; set; }
}

public class Variety
{
	[JsonPropertyName("pokemon")]
	public VarietyPokemon Pokemon { get; set; }
}

public class VarietyPokemon
{
	[JsonPropertyName("name")]
	public string Name { get; set; }
}

public class EvolutionChain
{
	[JsonPropertyName("chain")]
	public EvolutionNode Chain { get; set; }
}

public class EvolutionNode
{
	[JsonPropertyName("species")]
	public Species Species { get; set; }

	[JsonPropertyName("evolves_to")]
	public List<EvolutionNode> EvolvesTo { get; set; }
}

public class Species
{
	[JsonPropertyName("name")]
	public string Name { get; set; }
}

public class EvolutionChainModel
{
	[JsonPropertyName("chain")]
	public ChainLink Chain { get; set; }
}

public class ChainLink
{
	[JsonPropertyName("species")]
	public EvolutionSpecies Species { get; set; }

	[JsonPropertyName("evolves_to")]
	public List<ChainLink> EvolvesTo { get; set; }
}

public class EvolutionSpecies
{
	[JsonPropertyName("name")]
	public string Name { get; set; }

	[JsonPropertyName("url")]
	public string Url { get; set; }
}
public class PokemonListResponse
{
	[JsonPropertyName("results")]
	public List<PokemonListItem> Results { get; set; }
}

public class PokemonListItem
{
	[JsonPropertyName("name")]
	public string Name { get; set; }

	[JsonPropertyName("url")]
	public string Url { get; set; }
    public int Id { get; set; }
    public string ImageUrl { get; set; }
}
