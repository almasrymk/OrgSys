namespace FixedAssets.Domain.Exceptions;

public sealed class FixedAssetDomainException(string message) : Exception(message);
