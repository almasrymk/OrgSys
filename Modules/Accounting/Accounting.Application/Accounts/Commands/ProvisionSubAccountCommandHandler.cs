namespace Accounting.Application.Accounts.Commands
{
    using Accounting.Application.Accounts.Queries;
    using Accounting.Contracts.Accounts;
    using OrgSys.SharedKernel;
    using System.Net;

    public sealed class ProvisionSubAccountCommandHandler(
        IUnitOfWork unitOfWork,
        IRepository<Accounting.Domain.Account> repository) : ICommandHandler<ProvisionSubAccountCommand, AccountLookupDto>
    {
        public async Task<Result<AccountLookupDto>> Handle(ProvisionSubAccountCommand request, CancellationToken cancellationToken)
        {
            var parent = await repository.GetByFilterAsync(e => e.Id == request.ParentAccountId, string.Empty);
            if (parent is null || parent.Status == Status.Deleted || parent.Hide)
                return new Result<AccountLookupDto>(HttpStatusCode.BadRequest, null, [new Error("The configured parent account is missing or inactive.")]);

            var siblingCount = (await repository.GetListByFilterAsync(e => e.ParentId == parent.Id))?.Count() ?? 0;
            var code = $"{parent.Code}{(siblingCount + 1):00}";
            if (!long.TryParse(code, out var codeNumber))
                return new Result<AccountLookupDto>(HttpStatusCode.BadRequest, null, [new Error("Could not generate a numeric account code for the new sub-account.")]);

            var account = Accounting.Domain.Account.CreateSubAccount(parent, request.Name, code, codeNumber);
            await repository.CreateAsync(account);

            if (await unitOfWork.SaveChangeAsync(cancellationToken) <= 0)
                return new Result<AccountLookupDto>(HttpStatusCode.InternalServerError, null, [new Error("Error saving changes")]);

            return new Result<AccountLookupDto>(HttpStatusCode.OK, GetAccountQueryHandler.ToDto(account), null);
        }
    }
}
