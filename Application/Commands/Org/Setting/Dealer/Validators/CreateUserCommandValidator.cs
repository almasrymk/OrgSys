namespace Application.Commands.Org.Setting.Dealer.Validators
{
    using FluentValidation;
    using Domain.Abstraction;
    using Application.Validators;
    using Application.Commands.Org.Setting.Dealer.Commands;

    public class CreateDealerCommandValidator : Validator<CreateDealerCommand,  Entity.Model.Dealer>
    {
        public CreateDealerCommandValidator(IRepository<Entity.Model.Dealer> _Repository) : base(_Repository)
        {
            RuleFor(c => c.Name)
            .NotEmpty().WithMessage("The name field is required");

            RuleFor(c => c.Name)
            .MaximumLength(150).WithMessage("The name must not exceed 150 characters");

            RuleFor(c => c.Name)           
            .MustAsync(async (Name, cancellationToken) => await NotAnyAsync(c => c.Name == Name, cancellationToken))
            .WithMessage("The Dealer already exists")
            .OverridePropertyName(nameof(CreateDealerCommand.Name));
        }
    }
}