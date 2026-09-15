namespace Parties.Application.CustomerProfiles.Commands
{
    using Accounting.Contracts.Accounts;
    using Parties.Application.Dealers.Commands;
    using OrgSys.SharedKernel;
    using MediatR;
    using System.Net;

    /// <summary>
    /// Explicit business action (brief §2.15/CQRS RULE — not generic Create&lt;T&gt;): gives an
    /// existing Dealer the Customer role without creating a second, disconnected identity row
    /// (brief §2.9's mandatory "same company can be both customer and supplier" scenario). Reuses
    /// the exact same GL-account provisioning machinery Dealer's own Create/Update already use
    /// (DealerReceivableAccountProvisioning) rather than duplicating it.
    /// </summary>
    public sealed record AssignCustomerRoleCommand(
        long DealerId,
        long? AccountId,
        bool? AutoCreateReceivableAccount,
        decimal? CreditLimit) : ICommand;

    public sealed class AssignCustomerRoleCommandHandler(
        IUnitOfWork _UnitOfWork,
        IRepository<Parties.Domain.Dealer> _DealerRepository,
        IRepository<Parties.Domain.CustomerProfile> _Repository,
        IRepository<Preference> _PreferenceRepository,
        IReceivableAccountValidator _Validator,
        ISender sender) : ICommandHandler<AssignCustomerRoleCommand>
    {
        public async Task<Result> Handle(AssignCustomerRoleCommand request, CancellationToken cancellationToken)
        {
            var dealer = await _DealerRepository.GetByFilterAsync(
                e => e.Id == request.DealerId && e.Status != OrgSys.SharedKernel.Status.Deleted && e.Hide != true, string.Empty);
            if (dealer is null || dealer.Id == 0)
                return new Result(HttpStatusCode.BadRequest, [new Error("The party was not found.")]);

            // "Customer role cannot be assigned twice" (brief's PARTIES TEST EXAMPLES).
            if (await _Repository.AnyAsync(e => e.DealerId == request.DealerId, cancellationToken))
                return new Result(HttpStatusCode.BadRequest, [new Error("The customer role is already assigned to this party.")]);

            var (existingAccountId, provisionParentAccountId, errors) = await DealerReceivableAccountProvisioning.ResolveAsync(
                dealer, request.AccountId, request.AutoCreateReceivableAccount,
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

                await _Repository.CreateAsync(new Parties.Domain.CustomerProfile
                {
                    DealerId = request.DealerId,
                    AccountId = accountId,
                    CreditLimit = request.CreditLimit,
                    IsCreditAllowed = true
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
