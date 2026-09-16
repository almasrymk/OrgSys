namespace SaaS.Application.Tenants.Validators
{
    using SaaS.Application.Tenants.Commands;
    using OrgSys.SharedKernel;
    using FluentValidation;

    public class UpdateTenantCommandValidator : Validator<UpdateTenantCommand, Tenant>
    {
        public UpdateTenantCommandValidator(IRepository<Tenant> _Repository) : base(_Repository)
        {
            RuleFor(c => c.Name)
            .NotEmpty().WithMessage("The name field is required");

            RuleFor(c => c.Name)
            .MaximumLength(150).WithMessage("The name must not exceed 150 characters");

            RuleFor(c => c)
            .MustAsync(async (command, cancellationToken) => await NotAnyAsync(c => c.Name == command.Name && c.Id != command.Id, cancellationToken))
            .WithMessage("A tenant with this name already exists")
            .OverridePropertyName(nameof(UpdateTenantCommand.Name));
        }
    }
}
