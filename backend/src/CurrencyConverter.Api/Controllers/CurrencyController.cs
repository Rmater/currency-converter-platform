using Asp.Versioning;
using CurrencyConverter.Application.DTOs;
using CurrencyConverter.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CurrencyConverter.Api.Controllers;

[ApiController]
[ApiVersion("1.0")]
[Route("api/v{version:apiVersion}/currency")]
[Authorize(Policy = "ViewerOnly")]
public sealed class CurrencyController(ICurrencyService currencyService) : ControllerBase
{
    [HttpGet("latest")]
    public async Task<ActionResult<LatestRatesDto>> GetLatestRates([FromQuery] string @base = "EUR", CancellationToken cancellationToken = default)
    {
        return Ok(await currencyService.GetLatestRatesAsync(@base, cancellationToken));
    }

    [HttpPost("convert")]
    public async Task<ActionResult<ConversionResultDto>> Convert([FromBody] ConversionRequestDto request, CancellationToken cancellationToken)
    {
        return Ok(await currencyService.ConvertAsync(request, cancellationToken));
    }

    [HttpGet("history")]
    [Authorize(Policy = "AdminOnly")]
    public async Task<ActionResult<HistoricalRatesPageDto>> GetHistory(
        [FromQuery] string @base,
        [FromQuery] DateOnly startDate,
        [FromQuery] DateOnly endDate,
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 10,
        CancellationToken cancellationToken = default)
    {
        var result = await currencyService.GetHistoricalRatesAsync(new HistoricalRatesQueryDto(@base, startDate, endDate, page, pageSize), cancellationToken);
        return Ok(result);
    }
}
