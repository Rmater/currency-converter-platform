namespace CurrencyConverter.Application.DTOs;

public sealed record LatestRatesDto(string Base, DateOnly Date, IReadOnlyDictionary<string, decimal> Rates);
