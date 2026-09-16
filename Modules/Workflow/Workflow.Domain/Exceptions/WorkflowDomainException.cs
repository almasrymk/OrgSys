namespace Workflow.Domain.Exceptions;

public sealed class WorkflowDomainException(string message) : Exception(message);
