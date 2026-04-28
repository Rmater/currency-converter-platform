using CurrencyConverter.Domain.Entities;

namespace CurrencyConverter.Application.DTOs;

public sealed record HistoricalRatesPageDto(
    string Base,
    int Page,
    int PageSize,
    int TotalCount,
    IReadOnlyCollection<HistoricalRateEntry> Items);
