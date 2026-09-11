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

        [ForeignKey("Role")]
        public virtual long RoleId { get; set; }

        public virtual Role? Role { get; set; }

        [ForeignKey("Branch")]
        public virtual long? BranchId { get; set; }

        public virtual Branch? Branch { get; set; }

        public virtual long LoginUserId { get; set; }
    }
}