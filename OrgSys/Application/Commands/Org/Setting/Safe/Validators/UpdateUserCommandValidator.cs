namespace Application.Commands.Org.Setting.Safe.Validators
{
    using FluentValidation;
    using Domain.Abstraction;
    using Application.Validators;
    using Application.Commands.Org.Setting.Safe.Commands;
    using Utility;

    public class UpdateSafeCommandValidator : Validator<UpdateSafeCommand, Entity.Model.Safe>
    {
        public UpdateSafeCommandValidator(IRepository<Entity.Model.Safe> _Repository) : base(_Repository)
        {
            RuleFor(c => c.Name)
            .NotEmpty().WithMessage("The name field is required");

            RuleFor(c => c.Name)
            .MaximumLength(150).WithMessage("The name must not exceed 150 characters");

            RuleFor(c => new { c.Name, c.Id})
            .MustAsync(async (Ob, cancellationToken) => await NotAnyAsync(c => c.Name == Ob.Name && c.Id != Ob.Id && c.Status != Status.Deleted && c.Hide != true, cancellationToken))
            .WithMessage("The dealer group name already exists")
            .OverridePropertyName(nameof(UpdateSafeCommand.Name));
        }
    }
}