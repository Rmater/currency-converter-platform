using CurrencyConverter.Application.Interfaces;
using CurrencyConverter.Infrastructure.Clients;
using Microsoft.Extensions.DependencyInjection;

namespace CurrencyConverter.Infrastructure.Factories;

public sealed class CurrencyProviderFactory(IServiceProvider serviceProvider) : ICurrencyProviderFactory
{
    public IExchangeRateProvider Create(string providerKey = "frankfurter") => providerKey.ToLowerInvariant() switch
    {
        "frankfurter" => serviceProvider.GetRequiredService<FrankfurterApiClient>(),
        _ => throw new NotSupportedException($"Currency provider '{providerKey}' is not supported.")
    };
}
