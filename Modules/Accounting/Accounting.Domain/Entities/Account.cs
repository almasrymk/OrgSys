namespace Accounting.Domain
{
    using Accounting.Domain.Exceptions;

    [Table("Account")]
    public class Account : BaseModel
    {
        [StringLength(50, MinimumLength = 3)]
        public virtual string? Name { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public virtual decimal Debit { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public virtual decimal Credit { get; set; }

        [ForeignKey("AccountType")]
        public long AccountTypeId { get; set; }

        public virtual AccountType? AccountType { get; set; }

        /// <summary>False marks a group/parent account used only for hierarchy — Journal postings
        /// (including a Dealer's receivable account link) must target a postable/detail account.</summary>
        public virtual bool IsPostable { get; set; } = true;

        /// <summary>
        /// Marks a group/parent account as hierarchy-only (no journal line may ever post to it).
        /// The inverse, <see cref="MarkPostable"/>, is only meaningful for a leaf/detail account —
        /// callers are expected to have already established the account has no child accounts
        /// before flipping it back, but Account itself has no navigation to its children to check
        /// (that belongs to the Application/read-model layer, which already knows the tree).
        /// </summary>
        public void MarkNonPostable() => IsPostable = false;

        public void MarkPostable() => IsPostable = true;

        /// <summary>Hides the account from active use (Chart of Accounts pickers, new postings) without deleting its history.</summary>
        public void Deactivate() => Hide = true;

        public void Activate() => Hide = false;

        /// <summary>
        /// Renames the account. Kept as an explicit intent rather than a raw property setter for
        /// naming symmetry with the other lifecycle methods; shape/uniqueness validation (required,
        /// max length, unique code/name per AccountType) already lives in
        /// CreateAccountCommandValidator/UpdateAccountCommandValidator — not duplicated here per
        /// the migration brief's "don't duplicate the same rule in Validator and Domain" guidance.
        /// </summary>
        public void Rename(string name) => Name = name;

        public void ChangeAccountType(long accountTypeId) => AccountTypeId = accountTypeId;

        /// <summary>
        /// Enforces the "only a postable, active account may receive a journal line" rule referenced
        /// throughout the legacy posting bridges' comments but never previously checked in code —
        /// see the GeneralLedger migration report. Called by <see cref="Journal.AddLine"/>/
        /// <see cref="Journal.UpdateLine"/> and by <see cref="Journal.Post"/> for every referenced account.
        /// </summary>
        public void EnsurePostable()
        {
            if (Status == Status.Deleted)
                throw new AccountNotPostableException($"Account '{Name}' has been deleted and cannot receive postings.");

            if (Hide)
                throw new AccountNotPostableException($"Account '{Name}' is inactive and cannot receive postings.");

            if (!IsPostable)
                throw new AccountNotPostableException($"Account '{Name}' is a group/parent account and cannot receive postings directly.");
        }
    }
}
