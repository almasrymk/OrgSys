namespace Organization.Application.Branches.Validators
{
    using Organization.Application.Branches.Commands;
    using OrgSys.SharedKernel;
    using FluentValidation;

    /// <summary>Enforces brief §1.4's Branch invariants: a Branch must belong to a Company, its Name
    /// is unique within that Company, and an inactive (Hidden) Company cannot receive new Branches.</summary>
    public class CreateBranchCommandValidator : Validator<CreateBranchCommand, Organization.Domain.Branch>
    {
        public CreateBranchCommandValidator(
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

            RuleFor(c => c.CompanyId)
            .MustAsync(async (CompanyId, cancellationToken) => !await _CompanyRepository.AnyAsync(e => e.Id == CompanyId && e.Hide == true, cancellationToken))
            .WithMessage("An inactive company cannot receive new branches");

            RuleFor(c => c)
            .MustAsync(async (command, cancellationToken) => await NotAnyAsync(c => c.Name == command.Name && c.CompanyId == command.CompanyId, cancellationToken))
            .WithMessage("A branch with this name already exists in this company")
            .OverridePropertyName(nameof(CreateBranchCommand.Name));
        }
    }
}
