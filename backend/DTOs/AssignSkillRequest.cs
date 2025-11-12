namespace Backend.DTOs
{
    public class AssignSkillRequest
    {
        public int EmployeeId { get; set; }
        public List<int> SkillIds { get; set; } = new();
    }
}