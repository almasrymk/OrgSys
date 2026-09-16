namespace Tax.Domain.Exceptions;

public sealed class TaxDomainException(string message) : Exception(message);
