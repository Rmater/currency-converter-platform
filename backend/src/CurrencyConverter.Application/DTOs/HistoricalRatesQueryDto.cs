namespace CurrencyConverter.Application.DTOs;

public sealed record HistoricalRatesQueryDto(
    string Base,
    DateOnly StartDate,
    DateOnly EndDate,
    int Page = 1,
    int PageSize = 10);
