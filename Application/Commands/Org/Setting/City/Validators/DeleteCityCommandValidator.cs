namespace Application.Commands.Org.Setting.City.Validators
{
    using FluentValidation;
    using Domain.Abstraction;
    using Application.Validators;
    using Application.Commands.Org.Setting.City.Commands;

    public class DeleteCityCommandValidator : Validator<DeleteCityCommand, Domain.Entities.City>
    {
        public DeleteCityCommandValidator(IRepository<Domain.Entities.City> _Repository) : base(_Repository)
        {
            RuleFor(c => c.Id)
            .GreaterThan(0).WithMessage("The Id field is required");
          
            RuleFor(c => c)           
            .MustAsync(async (command, cancellationToken) => !await NotAnyAsync(c =>  c.Id == command.Id, cancellationToken))
            .WithMessage("The city not found");
        }
    }
}