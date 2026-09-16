namespace Budgeting.Domain.Exceptions;

public sealed class BudgetDomainException(string message) : Exception(message);
