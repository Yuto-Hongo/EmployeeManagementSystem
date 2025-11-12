namespace Backend.Models
{
    public class AuditLog
    {
        public int Id { get; set; }
        public int ActionUserId { get; set; }
        public string Action { get; set; }
        public string EntityName { get; set; }
        public int TargetId { get; set; }
        public DateTime Timestamp { get; set; }

        public string Details { get; set; }
    }
}