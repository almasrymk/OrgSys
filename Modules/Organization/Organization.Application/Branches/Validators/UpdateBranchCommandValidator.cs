namespace Organization.Application.Branches.Validators
{
    using Organization.Application.Branches.Commands;
    using OrgSys.SharedKernel;
    using FluentValidation;

    public class UpdateBranchCommandValidator : Validator<UpdateBranchCommand, Organization.Domain.Branch>
    {
        public UpdateBranchCommandValidator(
            IRepository<Organization.Domain.Branch> _Repository,
            IRepository<Organization.Domain.Company> _CompanyRepository) : base(_Repository)
        {
            RuleFor(c => c.Name)
            .NotEmpty().WithMessage("The name field is required");

            RuleFor(c => c.Name)
            .MaximumLength(50).WithMessage("The name must not exceed 50 characters");

            RuleFor(c => c.CompanyId)
            .GreaterThan(0).WithMessage("The company field is required");

            RuleFor(c => c.CompanyId)
            .MustAsync(async (CompanyId, cancellationToken) => await _CompanyRepository.AnyAsync(e => e.Id == CompanyId && e.Status != OrgSys.SharedKernel.Status.Deleted, cancellationToken))
            .WithMessage("The company not found");

            // Re-assigning a Branch to an already-inactive Company is blocked the same way creating
            // one under it is — moving an existing Branch under a Hidden Company would silently
            // reintroduce the same invariant violation Create blocks up front.
            RuleFor(c => c.CompanyId)
            .MustAsync(async (CompanyId, cancellationToken) => !await _CompanyRepository.AnyAsync(e => e.Id == CompanyId && e.Hide == true, cancellationToken))
            .WithMessage("An inactive company cannot receive branches");

            RuleFor(c => c)
            .MustAsync(async (command, cancellationToken) => await NotAnyAsync(c => c.Name == command.Name && c.CompanyId == command.CompanyId && c.Id != command.Id, cancellationToken))
            .WithMessage("A branch with this name already exists in this company")
            .OverridePropertyName(nameof(UpdateBranchCommand.Name));
        }
    }
}
