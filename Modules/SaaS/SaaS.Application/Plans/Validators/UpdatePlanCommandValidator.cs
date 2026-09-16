namespace SaaS.Application.Plans.Validators
{
    using SaaS.Application.Plans.Commands;
    using OrgSys.SharedKernel;
    using FluentValidation;

    public class UpdatePlanCommandValidator : Validator<UpdatePlanCommand, Plan>
    {
        public UpdatePlanCommandValidator(IRepository<Plan> _Repository) : base(_Repository)
        {
            RuleFor(c => c.Name).NotEmpty().WithMessage("The name field is required");
            RuleFor(c => c.Name).MaximumLength(100).WithMessage("The name must not exceed 100 characters");
            RuleFor(c => c.Price).GreaterThanOrEqualTo(0).WithMessage("Price must not be negative");

            RuleFor(c => c)
            .MustAsync(async (command, cancellationToken) => await NotAnyAsync(c => c.Name == command.Name && c.Id != command.Id, cancellationToken))
            .WithMessage("A plan with this name already exists")
            .OverridePropertyName(nameof(UpdatePlanCommand.Name));
        }
    }
}
