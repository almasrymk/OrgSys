namespace Application.Commands.Org.Setting.DealerGroup.Validators
{
    using FluentValidation;
    using Domain.Abstraction;
    using Application.Validators;
    using Application.Commands.Org.Setting.DealerGroup.Commands;
    using Utility;

    public class UpdateDealerGroupCommandValidator : Validator<UpdateDealerGroupCommand, Entity.Model.DealerGroup>
    {
        public UpdateDealerGroupCommandValidator(IRepository<Entity.Model.DealerGroup> _Repository) : base(_Repository)
        {
            RuleFor(c => c.Code)
            .NotEmpty().WithMessage("The code field is required");

            RuleFor(c => c.Name)
            .NotEmpty().WithMessage("The name field is required");

            RuleFor(c => c.Name)
            .MaximumLength(150).WithMessage("The name must not exceed 150 characters");

            RuleFor(c => new { c.Code, c.Id , c.TypeId })
           .MustAsync(async (Ob, cancellationToken) => await NotAnyAsync(c => c.Code == Ob.Code && c.TypeId == Ob.TypeId && c.Id != Ob.Id && c.Status != Status.Deleted  && c.Hide != true, cancellationToken))
           .WithMessage("The dealer group code already exists")
           .OverridePropertyName(nameof(UpdateDealerGroupCommand.Code));

            RuleFor(c => new { c.Name, c.Id , c.TypeId })
            .MustAsync(async (Ob, cancellationToken) => await NotAnyAsync(c => c.Name == Ob.Name && c.TypeId == Ob.TypeId  && c.Id != Ob.Id && c.Status != Status.Deleted && c.Hide != true, cancellationToken))
            .WithMessage("The dealer group name already exists")
            .OverridePropertyName(nameof(UpdateDealerGroupCommand.Name));
        }
    }
}