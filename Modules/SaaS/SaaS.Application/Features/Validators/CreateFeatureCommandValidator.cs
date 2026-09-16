namespace SaaS.Application.Features.Validators
{
    using SaaS.Application.Features.Commands;
    using OrgSys.SharedKernel;
    using FluentValidation;

    public class CreateFeatureCommandValidator : Validator<CreateFeatureCommand, Feature>
    {
        public CreateFeatureCommandValidator(IRepository<Feature> _Repository) : base(_Repository)
        {
            RuleFor(c => c.Key).NotEmpty().WithMessage("The key field is required");
            RuleFor(c => c.Name).NotEmpty().WithMessage("The name field is required");

            RuleFor(c => c)
            .MustAsync(async (command, cancellationToken) => await NotAnyAsync(c => c.Key == command.Key, cancellationToken))
            .WithMessage("A feature with this key already exists")
            .OverridePropertyName(nameof(CreateFeatureCommand.Key));
        }
    }
}
