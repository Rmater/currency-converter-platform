namespace CurrencyConverter.Application.DTOs;

public sealed record ConversionRequestDto(decimal Amount, string From, string To);
