namespace Application.Commands.Org.Setting.Dealer.Validators
{
    using Application.Commands.Org.Setting.Dealer.Commands;
    using Application.Validators;
    using Domain.Abstraction;
    using FluentValidation;
    using System.Xml.Linq;

    public class CreateDealerCommandValidator : Validator<CreateDealerCommand,  Domain.Entities.Dealer>
    {
        public CreateDealerCommandValidator(IRepository<Domain.Entities.Dealer> _Repository) : base(_Repository)
        {
            RuleFor(c => c.DealerGroupId)
            .NotEmpty().WithMessage("The dealer group field is required");

            RuleFor(c => c.Code)
            .NotEmpty().WithMessage("The code field is required");

            RuleFor(c => c.Name)
            .NotEmpty().WithMessage("The name field is required");

            RuleFor(c => c.Name)
            .MaximumLength(150).WithMessage("The name must not exceed 150 characters");

            RuleFor(c => c.Email)
           .EmailAddress().WithMessage("This is not email");

            RuleFor(c => new { c.Code , c.TypeId })
            .MustAsync(async (Ob, cancellationToken) => await NotAnyAsync(c => c.Code == Ob.Code && c.TypeId == Ob.TypeId && c.Status != Domain.Enums.Status.Deleted && c.Hide != true, cancellationToken))
            .WithMessage("The dealer code already exists")
            .OverridePropertyName(nameof(CreateDealerCommand.Code));

            RuleFor(c => new { c.Name, c.TypeId })           
            .MustAsync(async (Ob, cancellationToken) => await NotAnyAsync(c => c.Name == Ob.Name && c.TypeId == Ob.TypeId && c.Status != Domain.Enums.Status.Deleted && c.Hide != true, cancellationToken))
            .WithMessage("The dealer name already exists")
            .OverridePropertyName(nameof(CreateDealerCommand.Name));
        }
    }
}