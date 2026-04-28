namespace CurrencyConverter.Application.Interfaces;

public interface ICurrencyProviderFactory
{
    IExchangeRateProvider Create(string providerKey = "frankfurter");
}
