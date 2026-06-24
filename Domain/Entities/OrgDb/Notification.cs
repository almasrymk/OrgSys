namespace Domain.Entities
{
    [Table("Notification")]
    public class Notification : BaseModel
    {
        [StringLength(50, MinimumLength = 3)]
        public string Subject { get; set; }
        public string Message { get; set; }
        public string URL { get; set; }
        public DateTime Date { get; set; }
        public DateTime DisappearanceAfter { get; set; }
        public long FromUserId { get; set; }
        public long ToUserId { get; set; }
        public bool Read { get; set; }
    }
}