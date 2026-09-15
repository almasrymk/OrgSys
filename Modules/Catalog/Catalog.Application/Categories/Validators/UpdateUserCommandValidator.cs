namespace Catalog.Application.Categories.Validators
{
    using FluentValidation;
    using OrgSys.SharedKernel;
    using Catalog.Application.Categories.Commands;

    public class UpdateClassificationCommandValidator : Validator<UpdateClassificationCommand, Catalog.Domain.Classification>
    {
        public UpdateClassificationCommandValidator(IRepository<Catalog.Domain.Classification> _Repository) : base(_Repository)
        {
            RuleFor(c => c.Name)
            .NotEmpty().WithMessage("The name field is required");

            RuleFor(c => c.Name)
            .MaximumLength(150).WithMessage("The name must not exceed 150 characters");
             
            RuleFor(c => new { c.Name , c.Id})
            .MustAsync(async (Ob, cancellationToken) => await NotAnyAsync(c => c.Name == Ob.Name && c.Id != Ob.Id, cancellationToken))
            .WithMessage("The classification name already exists")
            .OverridePropertyName(nameof(UpdateClassificationCommand.Name)); 
        }
    }
}
