namespace Catalog.Application.Units.Validators
{
    using FluentValidation;
    using OrgSys.SharedKernel;
    using Catalog.Application.Units.Commands;

    public class UpdateUnitCommandValidator : Validator<UpdateUnitCommand, Catalog.Domain.Unit>
    {
        public UpdateUnitCommandValidator(IRepository<Catalog.Domain.Unit> _Repository) : base(_Repository)
        {
            RuleFor(c => c.Name)
            .NotEmpty().WithMessage("The name field is required");

            RuleFor(c => c.Name)
            .MaximumLength(150).WithMessage("The name must not exceed 150 characters");
             
            RuleFor(c => new { c.Name , c.Id})
            .MustAsync(async (Ob, cancellationToken) => await NotAnyAsync(c => c.Name == Ob.Name && c.Id != Ob.Id, cancellationToken))
            .WithMessage("The unit name already exists")
            .OverridePropertyName(nameof(UpdateUnitCommand.Name)); 
        }
    }
}
