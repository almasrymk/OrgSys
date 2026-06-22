namespace Application.Commands.Org.Setting.City.Validators
{
    using FluentValidation;
    using Domain.Abstraction;
    using Application.Validators;
    using Application.Commands.Org.Setting.City.Commands;

    public class DeleteListCityCommandValidator : Validator<DeleteListCityCommand, Domain.Entities.City>
    {
        public DeleteListCityCommandValidator(IRepository<Domain.Entities.City> _Repository) : base(_Repository)
        {    
            RuleFor(c => c)           
            .MustAsync(async (command, cancellationToken) => await IsAllIdsExist(command.Ids))
            .WithMessage("Some cities are not listed.");
        }

    }
}