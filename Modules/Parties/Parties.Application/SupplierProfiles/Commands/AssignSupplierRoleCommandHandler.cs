namespace Parties.Application.SupplierProfiles.Commands
{
    using Accounting.Contracts.Accounts;
    using Parties.Application.Dealers.Commands;
    using OrgSys.SharedKernel;
    using MediatR;
    using System.Net;

    /// <summary>Mirrors AssignCustomerRoleCommand for the Supplier role — see that type's doc
    /// comment. Reuses DealerPayableAccountProvisioning.</summary>
    public sealed record AssignSupplierRoleCommand(
        long DealerId,
        long? AccountId,
        bool? AutoCreatePayableAccount) : ICommand;

    public sealed class AssignSupplierRoleCommandHandler(
        IUnitOfWork _UnitOfWork,
        IRepository<Parties.Domain.Dealer> _DealerRepository,
        IRepository<Parties.Domain.SupplierProfile> _Repository,
        IRepository<Preference> _PreferenceRepository,
        IReceivableAccountValidator _Validator,
        ISender sender) : ICommandHandler<AssignSupplierRoleCommand>
    {
        public async Task<Result> Handle(AssignSupplierRoleCommand request, CancellationToken cancellationToken)
        {
            var dealer = await _DealerRepository.GetByFilterAsync(
                e => e.Id == request.DealerId && e.Status != OrgSys.SharedKernel.Status.Deleted && e.Hide != true, string.Empty);
            if (dealer is null || dealer.Id == 0)
                return new Result(HttpStatusCode.BadRequest, [new Error("The party was not found.")]);

            // "Supplier role cannot be assigned twice" (brief's PARTIES TEST EXAMPLES).
            if (await _Repository.AnyAsync(e => e.DealerId == request.DealerId, cancellationToken))
                return new Result(HttpStatusCode.BadRequest, [new Error("The supplier role is already assigned to this party.")]);

            var (existingAccountId, provisionParentAccountId, errors) = await DealerPayableAccountProvisioning.ResolveAsync(
                dealer, request.AccountId, request.AutoCreatePayableAccount,
                _Validator, _PreferenceRepository, sender, cancellationToken);
            if (errors.Count > 0)
                return new Result(HttpStatusCode.BadRequest, errors);

            await _UnitOfWork.BeginTransactionAsync();
            try
            {
                var accountId = existingAccountId;
                if (provisionParentAccountId is > 0)
                {
                    var provisionResult = await sender.Send(new ProvisionSubAccountCommand(provisionParentAccountId.Value, dealer.Name), cancellationToken);
                    if (provisionResult.Response is null)
                    {
                        await _UnitOfWork.RollbackAsync();
                        return new Result(HttpStatusCode.BadRequest, provisionResult.Errors);
                    }

                    accountId = provisionResult.Response.Id;
                }

                await _Repository.CreateAsync(new Parties.Domain.SupplierProfile
                {
                    DealerId = request.DealerId,
                    AccountId = accountId,
                    IsApproved = true
                });

                if (await _UnitOfWork.SaveChangeAsync(cancellationToken) <= 0)
                {
                    await _UnitOfWork.RollbackAsync();
                    return new Result(HttpStatusCode.InternalServerError, [new Error("Error")]);
                }

                await _UnitOfWork.CommitAsync();
                return new Result(HttpStatusCode.OK, null);
            }
            catch (Exception ex)
            {
                await _UnitOfWork.RollbackAsync();
                return new Result(HttpStatusCode.InternalServerError, [new Error(ex.Message)]);
            }
        }
    }
}
