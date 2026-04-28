namespace CurrencyConverter.Application.DTOs;

public sealed record ErrorResponseDto(string Message, string? CorrelationId = null);
