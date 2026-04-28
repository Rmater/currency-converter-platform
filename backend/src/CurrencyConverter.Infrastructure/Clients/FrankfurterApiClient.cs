using System.Net.Http.Json;
using System.Text.Json.Serialization;
using CurrencyConverter.Application.DTOs;
using CurrencyConverter.Application.Interfaces;
using CurrencyConverter.Domain.Entities;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;

namespace CurrencyConverter.Infrastructure.Clients;

public sealed class FrankfurterApiClient(
    HttpClient httpClient,
    IMemoryCache cache,
    ILogger<FrankfurterApiClient> logger) : IExchangeRateProvider
{
    public async Task<LatestRatesDto> GetLatestRatesAsync(string baseCurrency, CancellationToken cancellationToken)
    {
        var cacheKey = $"latest:{baseCurrency}";
        if (cache.TryGetValue(cacheKey, out LatestRatesDto? cached) && cached is not null)
        {
            return cached;
        }

        var response = await httpClient.GetFromJsonAsync<FrankfurterLatestResponse>($"latest?from={baseCurrency}", cancellationToken)
                      ?? throw new InvalidOperationException("No latest rates response was returned by the provider.");

        var result = new LatestRatesDto(response.Base, DateOnly.Parse(response.Date), response.Rates);
        cache.Set(cacheKey, result, TimeSpan.FromMinutes(5));
        return result;
    }

    public async Task<ConversionResultDto> ConvertAsync(decimal amount, string from, string to, CancellationToken cancellationToken)
    {
        var response = await httpClient.GetFromJsonAsync<FrankfurterConversionResponse>($"latest?amount={amount}&from={from}&to={to}", cancellationToken)
                      ?? throw new InvalidOperationException("No conversion response was returned by the provider.");

        var rate = response.Rates[to];
        return new ConversionResultDto(amount, from, to, rate / amount, rate, DateOnly.Parse(response.Date));
    }

    public async Task<IReadOnlyCollection<HistoricalRateEntry>> GetHistoricalRatesAsync(string baseCurrency, DateOnly startDate, DateOnly endDate, CancellationToken cancellationToken)
    {
        var cacheKey = $"history:{baseCurrency}:{startDate:yyyy-MM-dd}:{endDate:yyyy-MM-dd}";
        if (cache.TryGetValue(cacheKey, out IReadOnlyCollection<HistoricalRateEntry>? cached) && cached is not null)
        {
            return cached;
        }

        var response = await httpClient.GetFromJsonAsync<FrankfurterHistoricalResponse>($"{startDate:yyyy-MM-dd}..{endDate:yyyy-MM-dd}?from={baseCurrency}", cancellationToken)
                      ?? throw new InvalidOperationException("No historical response was returned by the provider.");

        var result = response.Rates
            .Select(x => new HistoricalRateEntry(DateOnly.Parse(x.Key), x.Value))
            .OrderBy(x => x.Date)
            .ToArray();

        cache.Set(cacheKey, result, TimeSpan.FromMinutes(10));
        logger.LogInformation("Historical rates loaded for {BaseCurrency} between {StartDate} and {EndDate}", baseCurrency, startDate, endDate);
        return result;
    }

    private class FrankfurterLatestResponse
    {
        [JsonPropertyName("amount")]
        public decimal Amount { get; init; }

        [JsonPropertyName("base")]
        public string Base { get; init; } = string.Empty;

        [JsonPropertyName("date")]
        public string Date { get; init; } = string.Empty;

        [JsonPropertyName("rates")]
        public Dictionary<string, decimal> Rates { get; init; } = [];
    }

    private sealed class FrankfurterConversionResponse : FrankfurterLatestResponse;

    private sealed class FrankfurterHistoricalResponse
    {
        [JsonPropertyName("amount")]
        public decimal Amount { get; init; }

        [JsonPropertyName("base")]
        public string Base { get; init; } = string.Empty;

        [JsonPropertyName("rates")]
        public Dictionary<string, Dictionary<string, decimal>> Rates { get; init; } = [];
    }
}
