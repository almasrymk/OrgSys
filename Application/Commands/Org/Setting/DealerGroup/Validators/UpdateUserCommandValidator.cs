namespace Application.Commands.Org.Setting.DealerGroup.Validators
{
    using FluentValidation;
    using Domain.Abstraction;
    using Application.Validators;
    using Application.Commands.Org.Setting.DealerGroup.Commands;

    public class UpdateDealerGroupCommandValidator : Validator<UpdateDealerGroupCommand, Entity.Model.DealerGroup>
    {
        public UpdateDealerGroupCommandValidator(IRepository<Entity.Model.DealerGroup> _Repository) : base(_Repository)
        {
            RuleFor(c => c.Name)
            .NotEmpty().WithMessage("The name field is required");

            RuleFor(c => c.Name)
            .MaximumLength(150).WithMessage("The name must not exceed 150 characters");

            RuleFor(c => new { c.Name, c.Id })
            .MustAsync(async (Ob, cancellationToken) => await NotAnyAsync(c => c.Name == Ob.Name && c.Id != Ob.Id, cancellationToken))
            .WithMessage("The role name already exists")
            .OverridePropertyName(nameof(UpdateDealerGroupCommand.Name));
        }
    }
}