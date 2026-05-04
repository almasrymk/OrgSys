namespace Application.Commands.Org.Setting.City.Validators
{
    using FluentValidation;
    using Domain.Abstraction;
    using Application.Validators;
    using Application.Commands.Org.Setting.City.Commands;

    public class CreateUserCommandValidator : Validator<CreateCityCommand ,  Entity.Model.City>
    {
        public CreateUserCommandValidator(IRepository<Entity.Model.City> _Repository , IRepository<Entity.Model.Country> _CountryRepository) : base(_Repository)
        {
            RuleFor(c => c.Name)
            .NotEmpty().WithMessage("The name field is required");

            RuleFor(c => c.Name)
            .MaximumLength(150).WithMessage("The name must not exceed 150 characters");

            RuleFor(c => c.CountryId)
            .NotEmpty().GreaterThan(0).WithMessage("The country field is required");

            RuleFor(c => c.CountryId)
           .MustAsync(async (CountryId, cancellationToken) => await _CountryRepository.AnyAsync(e=>e.Id == CountryId, cancellationToken))
            .WithMessage("The country not found");

            RuleFor(c => c)           
            .MustAsync(async (command, cancellationToken) => await NotAnyAsync(c => c.Name == command.Name && c.CountryId == command.CountryId, cancellationToken))
            .WithMessage("City already exists in this country");
        }
    }
}