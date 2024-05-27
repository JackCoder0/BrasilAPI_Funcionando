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
    public bool Verificacao { get; set; }
    public string? ErrorMessage { get; set; }
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

