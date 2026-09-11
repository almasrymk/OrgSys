namespace MasterData.Application.Cities.Validators
{
    using FluentValidation;
    using OrgSys.SharedKernel;
    using MasterData.Application.Cities.Commands;

    public class UpdateUserCommandValidator : Validator<UpdateCityCommand, MasterData.Domain.City>
    {
        public UpdateUserCommandValidator(IRepository<MasterData.Domain.City> _Repository , IRepository<MasterData.Domain.Country> _CountryRepository) : base(_Repository)
        {
            RuleFor(c => c.Id)
            .GreaterThan(0).WithMessage("The Id field is required");

            RuleFor(c => c.Name)
            .NotEmpty().WithMessage("The name field is required")
            .MaximumLength(150).WithMessage("The name must not exceed 150 characters");

            RuleFor(c => c.CountryId)
            .NotEmpty().GreaterThan(0).WithMessage("The country field is required");

            RuleFor(c => c.CountryId)
           .MustAsync(async (CountryId, cancellationToken) => await _CountryRepository.AnyAsync(e=>e.Id == CountryId, cancellationToken))
            .WithMessage("The country not found");

            RuleFor(c => c)           
            .MustAsync(async (command, cancellationToken) => await NotAnyAsync(c => c.Name == command.Name && c.CountryId == command.CountryId && c.Id != command.Id, cancellationToken))
            .WithMessage("City already exists in this country");
        }
    }
}
