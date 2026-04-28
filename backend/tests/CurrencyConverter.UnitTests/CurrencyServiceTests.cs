using CurrencyConverter.Application.DTOs;
using CurrencyConverter.Application.Interfaces;
using CurrencyConverter.Application.Services;
using CurrencyConverter.Domain.Entities;
using FluentAssertions;
using Moq;

namespace CurrencyConverter.UnitTests;

public sealed class CurrencyServiceTests
{
    private readonly Mock<ICurrencyProviderFactory> _factory = new();
    private readonly Mock<IExchangeRateProvider> _provider = new();

    public CurrencyServiceTests()
    {
        _factory.Setup(x => x.Create(It.IsAny<string>())).Returns(_provider.Object);
    }

    [Fact]
    public async Task ConvertAsync_ShouldRejectUnsupportedCurrency()
    {
        var service = new CurrencyService(_factory.Object);
        var action = async () => await service.ConvertAsync(new ConversionRequestDto(100, "EUR", "TRY"), CancellationToken.None);
        await action.Should().ThrowAsync<InvalidOperationException>();
    }

    [Fact]
    public async Task GetLatestRatesAsync_ShouldCallProvider()
    {
        var expected = new LatestRatesDto("EUR", new DateOnly(2026, 1, 1), new Dictionary<string, decimal> { ["USD"] = 1.1m });
        _provider.Setup(x => x.GetLatestRatesAsync("EUR", It.IsAny<CancellationToken>())).ReturnsAsync(expected);

        var service = new CurrencyService(_factory.Object);
        var actual = await service.GetLatestRatesAsync("EUR", CancellationToken.None);

        actual.Should().BeEquivalentTo(expected);
    }

    [Fact]
    public async Task GetHistoricalRatesAsync_ShouldPaginateResults()
    {
        var entries = Enumerable.Range(1, 20)
            .Select(i => new HistoricalRateEntry(new DateOnly(2024, 1, i), new Dictionary<string, decimal> { ["USD"] = 1.1m + i }))
            .ToArray();

        _provider.Setup(x => x.GetHistoricalRatesAsync("EUR", It.IsAny<DateOnly>(), It.IsAny<DateOnly>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(entries);

        var service = new CurrencyService(_factory.Object);
        var result = await service.GetHistoricalRatesAsync(new HistoricalRatesQueryDto("EUR", new DateOnly(2024, 1, 1), new DateOnly(2024, 1, 20), 2, 5), CancellationToken.None);

        result.TotalCount.Should().Be(20);
        result.Items.Should().HaveCount(5);
    }
}
