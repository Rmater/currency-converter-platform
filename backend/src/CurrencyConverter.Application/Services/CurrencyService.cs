using CurrencyConverter.Application.DTOs;
using CurrencyConverter.Application.Interfaces;
using CurrencyConverter.Domain.Constants;

namespace CurrencyConverter.Application.Services;

public sealed class CurrencyService(ICurrencyProviderFactory providerFactory) : ICurrencyService
{
    public async Task<LatestRatesDto> GetLatestRatesAsync(string baseCurrency, CancellationToken cancellationToken)
    {
        ValidateCurrency(baseCurrency, nameof(baseCurrency));
        return await providerFactory.Create().GetLatestRatesAsync(baseCurrency.ToUpperInvariant(), cancellationToken);
    }

    public async Task<ConversionResultDto> ConvertAsync(ConversionRequestDto request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);
        if (request.Amount <= 0)
        {
            throw new ArgumentException("Amount must be greater than zero.");
        }

        ValidateCurrency(request.From, nameof(request.From));
        ValidateCurrency(request.To, nameof(request.To));

        if (string.Equals(request.From, request.To, StringComparison.OrdinalIgnoreCase))
        {
            throw new ArgumentException("Source and target currencies must be different.");
        }

        return await providerFactory.Create().ConvertAsync(
            request.Amount,
            request.From.ToUpperInvariant(),
            request.To.ToUpperInvariant(),
            cancellationToken);
    }

    public async Task<HistoricalRatesPageDto> GetHistoricalRatesAsync(HistoricalRatesQueryDto query, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(query);
        ValidateCurrency(query.Base, nameof(query.Base));

        if (query.Page <= 0 || query.PageSize <= 0)
        {
            throw new ArgumentException("Page and page size must be positive numbers.");
        }

        if (query.StartDate > query.EndDate)
        {
            throw new ArgumentException("Start date must be before or equal to end date.");
        }

        var items = await providerFactory.Create().GetHistoricalRatesAsync(
            query.Base.ToUpperInvariant(),
            query.StartDate,
            query.EndDate,
            cancellationToken);

        var paginatedItems = items
            .OrderByDescending(x => x.Date)
            .Skip((query.Page - 1) * query.PageSize)
            .Take(query.PageSize)
            .ToArray();

        return new HistoricalRatesPageDto(query.Base.ToUpperInvariant(), query.Page, query.PageSize, items.Count, paginatedItems);
    }

    private static void ValidateCurrency(string? currency, string parameterName)
    {
        if (string.IsNullOrWhiteSpace(currency) || currency.Length != 3)
        {
            throw new ArgumentException("Currency code must be a valid ISO 4217 3-letter code.", parameterName);
        }

        if (UnsupportedCurrencies.Values.Contains(currency))
        {
            throw new InvalidOperationException($"Currency {currency.ToUpperInvariant()} is not supported.");
        }
    }
}
