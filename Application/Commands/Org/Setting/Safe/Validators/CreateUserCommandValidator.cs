namespace Application.Commands.Org.Setting.Safe.Validators
{
    using Application.Commands.Org.Setting.Safe.Commands;
    using Application.Validators;
    using Domain.Abstraction;
    using FluentValidation;
    using Utility;

    public class CreateSafeCommandValidator : Validator<CreateSafeCommand,  Entity.Model.Safe>
    {
        public CreateSafeCommandValidator(IRepository<Entity.Model.Safe> _Repository) : base(_Repository)
        {
            RuleFor(c => c.Code)
           .NotEmpty().WithMessage("The code field is required");

            RuleFor(c => c.Name)
            .NotEmpty().WithMessage("The name field is required");

            RuleFor(c => c.Name)
            .MaximumLength(150).WithMessage("The name must not exceed 150 characters");

            RuleFor(c => new { c.Code })
            .MustAsync(async (Ob, cancellationToken) => await NotAnyAsync(c => c.Code == Ob.Code && c.Status != Status.Deleted && c.Hide != true, cancellationToken))
            .WithMessage("The safe code already exists")
            .OverridePropertyName(nameof(CreateSafeCommand.Code));

            RuleFor(c => new { c.Name})           
            .MustAsync(async (Ob, cancellationToken) => await NotAnyAsync(c => c.Name == Ob.Name && c.Status != Status.Deleted && c.Hide != true, cancellationToken))
            .WithMessage("The safe name already exists")
            .OverridePropertyName(nameof(CreateSafeCommand.Name));
        }
    }
}