namespace Application.Commands.Org.Setting.Account.Validators
{
    using FluentValidation;
    using Domain.Abstraction;
    using Application.Validators;
    using Application.Commands.Org.Setting.Account.Commands;

    public class UpdateAccountCommandValidator : Validator<UpdateAccountCommand, Domain.Entities.Account>
    {
        public UpdateAccountCommandValidator(IRepository<Domain.Entities.Account> _Repository) : base(_Repository)
        {
            RuleFor(c => c.AccountTypeId)
            .NotEmpty().GreaterThanOrEqualTo(0).WithMessage("The account type field is required");

            RuleFor(c => c.Code)
            .NotEmpty().WithMessage("The code field is required");

            RuleFor(c => c.Name)
            .NotEmpty().WithMessage("The name field is required");

            RuleFor(c => c.Name)
            .MaximumLength(150).WithMessage("The name must not exceed 150 characters");
             
            RuleFor(c => new { c.Code , c.Id, c.TypeId })
            .MustAsync(async (Ob, cancellationToken) => await NotAnyAsync(c => c.Code == Ob.Code && c.Id != Ob.Id && c.TypeId == Ob.TypeId && c.Status != Domain.Enums.Status.Deleted && c.Hide != true, cancellationToken))
            .WithMessage("The account code already exists")
            .OverridePropertyName(nameof(CreateAccountCommand.Code));

            RuleFor(c => new { c.Name, c.Id, c.TypeId })
            .MustAsync(async (Ob, cancellationToken) => await NotAnyAsync(c => c.Name == Ob.Name && c.Id != Ob.Id && c.TypeId == Ob.TypeId && c.Status != Domain.Enums.Status.Deleted && c.Hide != true, cancellationToken))
            .WithMessage("The account name already exists")
            .OverridePropertyName(nameof(CreateAccountCommand.Name));
        }
    }
}