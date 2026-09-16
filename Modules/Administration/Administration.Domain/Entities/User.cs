namespace Administration.Domain
{
    [Table("User")]
    public class User : BaseModel
    {
        [Required]
        public virtual string Name { get; set; } = null!;

        [Required]
        public virtual string UserName { get; set; } = null!;
       
        public virtual string? Password { get; set; }

        /// <summary>
        /// When true, login is rejected until an administrator (or a password-reset flow)
        /// stores a new Identity hash. Existing rows were AES ciphertext before the hasher
        /// cutover and cannot be verified — see migration AddUserPasswordHashing.
        /// </summary>
        public virtual bool MustResetPassword { get; set; }

        [ForeignKey("Role")]
        public virtual long RoleId { get; set; }

        public virtual Role? Role { get; set; }

        /// <summary>Scalar-only reference into Organization.Domain.Branch — no EF navigation.
        /// FK preserved via Fluent HasOne(typeof(Branch)) in OrgContext.</summary>
        public virtual long? BranchId { get; set; }

        public virtual long LoginUserId { get; set; }
    }
}