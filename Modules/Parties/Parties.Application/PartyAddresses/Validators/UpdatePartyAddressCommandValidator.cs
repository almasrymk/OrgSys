namespace Parties.Application.PartyAddresses.Validators
{
    using Parties.Application.PartyAddresses.Commands;
    using OrgSys.SharedKernel;
    using FluentValidation;

    public class UpdatePartyAddressCommandValidator : Validator<UpdatePartyAddressCommand, Parties.Domain.PartyAddress>
    {
        public UpdatePartyAddressCommandValidator(
            IRepository<Parties.Domain.PartyAddress> _Repository,
            IRepository<Parties.Domain.Dealer> _DealerRepository) : base(_Repository)
        {
            RuleFor(c => c.DealerId)
            .MustAsync(async (DealerId, cancellationToken) => await _DealerRepository.AnyAsync(e => e.Id == DealerId, cancellationToken))
            .WithMessage("The party not found");

            RuleFor(c => c.Line1)
            .NotEmpty().WithMessage("The address line field is required");
        }
    }
}
