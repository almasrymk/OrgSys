namespace Application.Commands.Org.Setting.City.Validators
{
    using FluentValidation;
    using Domain.Abstraction;
    using Application.Commands.Org.Setting.City.Commands;

    public class DeleteListCityCommandValidator : Validator<DeleteListCityCommand, Entity.Model.City>
    {
        public DeleteListCityCommandValidator(IRepository<Entity.Model.City> _Repository) : base(_Repository)
        {    
            RuleFor(c => c)           
            .MustAsync(async (command, cancellationToken) => await IsAllIdsExist(command.Ids))
            .WithMessage("Some cities are not listed.");
        }

    }
}