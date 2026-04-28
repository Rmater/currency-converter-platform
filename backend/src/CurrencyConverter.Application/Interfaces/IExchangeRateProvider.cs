using CurrencyConverter.Application.DTOs;
using CurrencyConverter.Domain.Entities;

namespace CurrencyConverter.Application.Interfaces;

public interface IExchangeRateProvider
{
    Task<LatestRatesDto> GetLatestRatesAsync(string baseCurrency, CancellationToken cancellationToken);
    Task<ConversionResultDto> ConvertAsync(decimal amount, string from, string to, CancellationToken cancellationToken);
    Task<IReadOnlyCollection<HistoricalRateEntry>> GetHistoricalRatesAsync(string baseCurrency, DateOnly startDate, DateOnly endDate, CancellationToken cancellationToken);
}
