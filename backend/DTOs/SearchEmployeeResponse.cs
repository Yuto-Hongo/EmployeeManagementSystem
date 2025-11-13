using Backend.Enums;

namespace Backend.DTOs
{
    public class SearchEmployeeResponse
    {
        public int Id { get; set; }
        public string FullName { get; set; } = string.Empty;
        public DateTime DateOfBirth { get; set; }
        public Gender Gender { get; set; }
        public string Address { get; set; } = string.Empty;
        public DateTime JoinDate { get; set; }
        public string CurrentWorkplace { get; set; } = string.Empty;

        public UserInfoDto? User { get; set; }
        public List<SkillInfoDto> Skills { get; set; } = new();
    }

    public class SearchUserInfoDto
    {
        public string Email { get; set; } = string.Empty;
        public UserRole Role { get; set; }
    }

    public class SearchSkillInfoDto
    {
        public string Name { get; set; } = string.Empty;
        public string Category { get; set; } = string.Empty;
        public string IconPath { get; set; } = string.Empty;
    }
}

