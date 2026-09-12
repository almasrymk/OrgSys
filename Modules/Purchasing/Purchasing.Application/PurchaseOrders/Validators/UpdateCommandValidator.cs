namespace Purchasing.Application.PurchaseOrders.Validators
{
    using OrgSys.SharedKernel;
    using FluentValidation;
    using Purchasing.Application.PurchaseOrders.Commands;

    public class UpdatePurchaseOrderCommandValidator : Validator<UpdatePurchaseOrderCommand, PurchaseOrder>
    {
        public UpdatePurchaseOrderCommandValidator(IRepository<PurchaseOrder> _Repository) : base(_Repository)
        {
            RuleFor(c => c.DealerId)
            .NotEmpty().WithMessage("The supplier field is required");

            RuleFor(c => c.Code)
            .NotEmpty().WithMessage("The code field is required");

            RuleFor(c => c.PurchaseOrderProductList)
            .NotEmpty().WithMessage("At least one product line is required");

            RuleFor(c => new { c.Code, c.Id, c.TypeId })
            .MustAsync(async (Ob, cancellationToken) => await NotAnyAsync(c => c.Code == Ob.Code && c.TypeId == Ob.TypeId && c.Id != Ob.Id && c.Status != Status.Deleted && c.Hide != true, cancellationToken))
            .WithMessage("The purchase order code already exists")
            .OverridePropertyName(nameof(UpdatePurchaseOrderCommand.Code));
        }
    }
}
