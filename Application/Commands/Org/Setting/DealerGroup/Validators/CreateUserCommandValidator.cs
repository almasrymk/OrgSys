namespace Application.Commands.Org.Setting.DealerGroup.Validators
{
    using Application.Commands.Org.Setting.DealerGroup.Commands;
    using Application.Validators;
    using Domain.Abstraction;
    using FluentValidation;
    using System.Xml.Linq;
    using Utility;

    public class CreateDealerGroupCommandValidator : Validator<CreateDealerGroupCommand,  Entity.Model.DealerGroup>
    {
        public CreateDealerGroupCommandValidator(IRepository<Entity.Model.DealerGroup> _Repository) : base(_Repository)
        {
            RuleFor(c => c.Code)
           .NotEmpty().WithMessage("The code field is required");

            RuleFor(c => c.Name)
            .NotEmpty().WithMessage("The name field is required");

            RuleFor(c => c.Name)
            .MaximumLength(150).WithMessage("The name must not exceed 150 characters");

            RuleFor(c => new { c.Code, c.TypeId })
            .MustAsync(async (Ob, cancellationToken) => await NotAnyAsync(c => c.Code == Ob.Code && c.TypeId == Ob.TypeId && c.Status != Status.Deleted && c.Hide != true, cancellationToken))
            .WithMessage("The dealer group code already exists")
            .OverridePropertyName(nameof(CreateDealerGroupCommand.Code));

            RuleFor(c => new { c.Name, c.TypeId })           
            .MustAsync(async (Ob, cancellationToken) => await NotAnyAsync(c => c.Name == Ob.Name && c.TypeId == Ob.TypeId && c.Status != Status.Deleted && c.Hide != true, cancellationToken))
            .WithMessage("The dealer group name already exists")
            .OverridePropertyName(nameof(CreateDealerGroupCommand.Name));
        }
    }
}