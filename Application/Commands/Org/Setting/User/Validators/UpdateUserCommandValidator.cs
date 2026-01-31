namespace Application.Commands.Org.Setting.User.Validators
{
    using FluentValidation;
    using Domain.Abstraction;
    using Application.Validators;
    using Application.Commands.Org.Setting.User.Commands;

    public class UpdateUserCommandValidator : Validator<UpdateUserCommand, Entity.Model.User>
    {
        public UpdateUserCommandValidator(IRepository<Entity.Model.User> _Repository) : base(_Repository)
        {
            RuleFor(c => c.Name)
            .NotEmpty().WithMessage("The name field is required");

            RuleFor(c => c.Name)
            .MaximumLength(150).WithMessage("The name must not exceed 150 characters");

            RuleFor(c => c.RoleId)
            .NotEmpty().GreaterThan(0).WithMessage("The role field is required");

            RuleFor(c => new { c.Name , c.Id})
            .MustAsync(async (Ob, cancellationToken) => await NotAnyAsync(c => c.Name == Ob.Name && c.Id != Ob.Id, cancellationToken))
            .WithMessage("The role name already exists")
            .OverridePropertyName(nameof(UpdateUserCommand.Name));

            RuleFor(c => new { c.UserName, c.Id})
            .MustAsync(async (Ob, cancellationToken) => await NotAnyAsync(c => c.UserName == Ob.UserName && c.Id != Ob.Id, cancellationToken))
            .WithMessage("The username already exists")
            .OverridePropertyName(nameof(UpdateUserCommand.UserName));
        }
    }
}