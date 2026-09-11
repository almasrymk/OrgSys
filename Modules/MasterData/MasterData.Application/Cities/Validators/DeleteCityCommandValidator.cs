namespace MasterData.Application.Cities.Validators
{
    using FluentValidation;
    using OrgSys.SharedKernel;
    using MasterData.Application.Cities.Commands;

    public class DeleteCityCommandValidator : Validator<DeleteCityCommand, MasterData.Domain.City>
    {
        public DeleteCityCommandValidator(IRepository<MasterData.Domain.City> _Repository) : base(_Repository)
        {
            RuleFor(c => c.Id)
            .GreaterThan(0).WithMessage("The Id field is required");
          
            RuleFor(c => c)           
            .MustAsync(async (command, cancellationToken) => !await NotAnyAsync(c =>  c.Id == command.Id, cancellationToken))
            .WithMessage("The city not found");
        }
    }
}
