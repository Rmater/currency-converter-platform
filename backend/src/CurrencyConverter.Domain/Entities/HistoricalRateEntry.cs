namespace CurrencyConverter.Domain.Entities;

public sealed record HistoricalRateEntry(DateOnly Date, IReadOnlyDictionary<string, decimal> Rates);
