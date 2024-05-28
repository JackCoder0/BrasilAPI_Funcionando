namespace Teste_BRASILAPI.Extensions;

public static class LetraMaiuscula
{
    public static string PrimeiraLetra(this string input)
    {
        if (string.IsNullOrEmpty(input))
            return input;

        return char.ToUpper(input[0]) + input.Substring(1).ToLower();
    }
}
