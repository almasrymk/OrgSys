namespace Purchasing.Application.PurchaseRequisitions.Validators
{
    using OrgSys.SharedKernel;
    using FluentValidation;
    using Purchasing.Application.PurchaseRequisitions.Commands;

    public class CreatePurchaseRequisitionCommandValidator : Validator<CreatePurchaseRequisitionCommand, PurchaseRequisition>
    {
        public CreatePurchaseRequisitionCommandValidator(IRepository<PurchaseRequisition> _Repository) : base(_Repository)
        {
            RuleFor(c => c.Code)
            .NotEmpty().WithMessage("The code field is required");

            RuleFor(c => c.PurchaseRequisitionProductList)
            .NotEmpty().WithMessage("At least one product line is required");

            RuleFor(c => new { c.Code, c.TypeId })
            .MustAsync(async (Ob, cancellationToken) => await NotAnyAsync(c => c.Code == Ob.Code && c.TypeId == Ob.TypeId && c.Status != Status.Deleted && c.Hide != true, cancellationToken))
            .WithMessage("The purchase requisition code already exists")
            .OverridePropertyName(nameof(CreatePurchaseRequisitionCommand.Code));
        }
    }
}
