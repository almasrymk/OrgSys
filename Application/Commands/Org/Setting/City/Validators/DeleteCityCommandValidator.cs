namespace Application.Commands.Org.Setting.City.Validators
{
    using FluentValidation;
    using Domain.Abstraction;
    using Application.Commands.Org.Setting.City.Commands;

    public class DeleteCityCommandValidator : Validator<DeleteCityCommand, Entity.Model.City>
    {
        public DeleteCityCommandValidator(IRepository<Entity.Model.City> _Repository) : base(_Repository)
        {
            RuleFor(c => c.Id)
            .GreaterThan(0).WithMessage("The Id field is required");
          
            RuleFor(c => c)           
            .MustAsync(async (command, cancellationToken) => !await NotAnyAsync(c =>  c.Id == command.Id, cancellationToken))
            .WithMessage("The city not found");
        }
    }
}