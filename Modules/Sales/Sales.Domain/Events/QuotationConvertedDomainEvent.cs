namespace Sales.Domain.Events;

/// <summary>Raised by Quotation.MarkConverted once the caller (Sales.Application) has created the
/// resulting SalesOrder — SalesOrderId is referenced only, never navigated (brief §12: conversion
/// creates a separate document; the Quotation stays historical, never mutated into a SalesOrder).</summary>
public sealed record QuotationConvertedDomainEvent(long QuotationId, long SalesOrderId) : IDomainEvent
{
    public DateTime OccurredOn { get; } = DateTime.UtcNow;
}
