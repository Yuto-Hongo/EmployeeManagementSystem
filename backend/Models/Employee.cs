using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using Backend.Enums;

namespace Backend.Models
{
    public class Employee
    {
        [Key]
        public int Id { get; set; }

        public int UserId { get; set; } 

        public string? FullName { get; set; }

        [DataType(DataType.Date)]
        public DateTime? DateOfBirth { get; set; }

        [JsonConverter(typeof(JsonStringEnumConverter))]
        public Gender? Gender { get; set; }

        public string? Address { get; set; }

        [DataType(DataType.Date)]
        public DateTime? JoinDate { get; set; }

        public int? VacationRemaining { get; set; }

        public string? CurrentWorkplace { get; set; }

        public User User { get; set; }

        public ICollection<EmployeeSkill> EmployeeSkills { get; set; } = new List<EmployeeSkill>();
    }
}