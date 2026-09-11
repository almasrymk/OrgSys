namespace MasterData.Application.Classifications.Validators
{
    using FluentValidation;
    using OrgSys.SharedKernel;
    using MasterData.Application.Classifications.Commands;

    public class UpdateClassificationCommandValidator : Validator<UpdateClassificationCommand, MasterData.Domain.Classification>
    {
        public UpdateClassificationCommandValidator(IRepository<MasterData.Domain.Classification> _Repository) : base(_Repository)
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
