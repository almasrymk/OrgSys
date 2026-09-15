namespace Parties.Application.PartyContacts.Validators
{
    using Parties.Application.PartyContacts.Commands;
    using OrgSys.SharedKernel;
    using FluentValidation;

    public class UpdatePartyContactCommandValidator : Validator<UpdatePartyContactCommand, Parties.Domain.PartyContact>
    {
        public UpdatePartyContactCommandValidator(
            IRepository<Parties.Domain.PartyContact> _Repository,
            IRepository<Parties.Domain.Dealer> _DealerRepository) : base(_Repository)
        {
            RuleFor(c => c.Name)
            .NotEmpty().WithMessage("The name field is required");

            RuleFor(c => c.DealerId)
            .MustAsync(async (DealerId, cancellationToken) => await _DealerRepository.AnyAsync(e => e.Id == DealerId, cancellationToken))
            .WithMessage("The party not found");

            RuleFor(c => c.Email)
            .EmailAddress().When(c => !string.IsNullOrEmpty(c.Email)).WithMessage("This is not a valid email");
        }
    }
}
