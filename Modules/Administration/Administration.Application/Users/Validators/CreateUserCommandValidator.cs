namespace Administration.Application.Users.Validators
{
    using FluentValidation;
    using OrgSys.SharedKernel;
    using Administration.Application.Users.Commands;

    public class CreateUserCommandValidator : Validator<CreateUserCommand,  Administration.Domain.User>
    {
        public CreateUserCommandValidator(IRepository<Administration.Domain.User> _Repository) : base(_Repository)
        {
            RuleFor(c => c.Name)
            .NotEmpty().WithMessage("The name field is required");

            RuleFor(c => c.Name)
            .MaximumLength(150).WithMessage("The name must not exceed 150 characters");

            RuleFor(c => c.RoleId)
            .NotEmpty().GreaterThan(0).WithMessage("The role field is required");

            RuleFor(c => c.Name)           
            .MustAsync(async (Name, cancellationToken) => await NotAnyAsync(c => c.Name == Name, cancellationToken))
            .WithMessage("The user already exists")
            .OverridePropertyName(nameof(CreateUserCommand.Name));

            RuleFor(c => c.UserName)
            .MustAsync(async (UserName, cancellationToken) => await NotAnyAsync(c => c.UserName == UserName, cancellationToken))
            .WithMessage("The username already exists")
            .OverridePropertyName(nameof(CreateUserCommand.UserName));

        }
    }
}