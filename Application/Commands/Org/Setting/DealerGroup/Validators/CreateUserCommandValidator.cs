namespace Application.Commands.Org.Setting.DealerGroup.Validators
{
    using FluentValidation;
    using Domain.Abstraction;
    using Application.Validators;
    using Application.Commands.Org.Setting.DealerGroup.Commands;

    public class CreateDealerGroupCommandValidator : Validator<CreateDealerGroupCommand,  Entity.Model.DealerGroup>
    {
        public CreateDealerGroupCommandValidator(IRepository<Entity.Model.DealerGroup> _Repository) : base(_Repository)
        {
            RuleFor(c => c.Name)
            .NotEmpty().WithMessage("The name field is required");

            RuleFor(c => c.Name)
            .MaximumLength(150).WithMessage("The name must not exceed 150 characters");
             
            RuleFor(c => c.Name)           
            .MustAsync(async (Name, cancellationToken) => await NotAnyAsync(c => c.Name == Name, cancellationToken))
            .WithMessage("The DealerGroup already exists")
            .OverridePropertyName(nameof(CreateDealerGroupCommand.Name));
        }
    }
}