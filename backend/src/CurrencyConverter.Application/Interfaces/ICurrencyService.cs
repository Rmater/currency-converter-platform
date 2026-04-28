using CurrencyConverter.Application.DTOs;

namespace CurrencyConverter.Application.Interfaces;

public interface ICurrencyService
{
    Task<LatestRatesDto> GetLatestRatesAsync(string baseCurrency, CancellationToken cancellationToken);
    Task<ConversionResultDto> ConvertAsync(ConversionRequestDto request, CancellationToken cancellationToken);
    Task<HistoricalRatesPageDto> GetHistoricalRatesAsync(HistoricalRatesQueryDto query, CancellationToken cancellationToken);
}
