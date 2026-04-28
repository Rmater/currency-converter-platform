namespace CurrencyConverter.Application.DTOs;

public sealed record ConversionResultDto(decimal Amount, string From, string To, decimal Rate, decimal ConvertedAmount, DateOnly Date);
