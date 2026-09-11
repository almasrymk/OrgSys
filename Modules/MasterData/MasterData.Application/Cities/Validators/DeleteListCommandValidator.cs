namespace MasterData.Application.Cities.Validators
{
    using FluentValidation;
    using OrgSys.SharedKernel;
    using MasterData.Application.Cities.Commands;

    public class DeleteListCityCommandValidator : Validator<DeleteListCityCommand, MasterData.Domain.City>
    {
        public DeleteListCityCommandValidator(IRepository<MasterData.Domain.City> _Repository) : base(_Repository)
        {    
            RuleFor(c => c)           
            .MustAsync(async (command, cancellationToken) => await IsAllIdsExist(command.Ids))
            .WithMessage("Some cities are not listed.");
        }

    }
}
