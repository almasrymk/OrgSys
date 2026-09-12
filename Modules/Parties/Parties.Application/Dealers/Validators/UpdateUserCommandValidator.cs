namespace Parties.Application.Dealers.Validators
{
    using FluentValidation;
    using OrgSys.SharedKernel;
    using Parties.Application.Dealers.Commands;

    public class UpdateDealerCommandValidator : Validator<UpdateDealerCommand, Parties.Domain.Dealer>
    {
        public UpdateDealerCommandValidator(IRepository<Parties.Domain.Dealer> _Repository) : base(_Repository)
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

            RuleFor(c => new { c.Code , c.Id, c.TypeId })
            .MustAsync(async (Ob, cancellationToken) => await NotAnyAsync(c => c.Code == Ob.Code && c.Id != Ob.Id && c.TypeId == Ob.TypeId && c.Status != OrgSys.SharedKernel.Status.Deleted && c.Hide != true, cancellationToken))
            .WithMessage("The dealer code already exists")
            .OverridePropertyName(nameof(CreateDealerCommand.Code));

            RuleFor(c => new { c.Name, c.Id, c.TypeId })
            .MustAsync(async (Ob, cancellationToken) => await NotAnyAsync(c => c.Name == Ob.Name && c.Id != Ob.Id && c.TypeId == Ob.TypeId && c.Status != OrgSys.SharedKernel.Status.Deleted && c.Hide != true, cancellationToken))
            .WithMessage("The dealer name already exists")
            .OverridePropertyName(nameof(CreateDealerCommand.Name));
        }
    }
}