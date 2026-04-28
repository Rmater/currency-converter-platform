namespace CurrencyConverter.Domain.Constants;

public static class UnsupportedCurrencies
{
    public static readonly HashSet<string> Values = new(StringComparer.OrdinalIgnoreCase)
    {
        "TRY", "PLN", "THB", "MXN"
    };
}
