using Backend.Enums;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Backend.DTOs
{
    public class EditEmployeeRequest
    {
        public string? FullName { get; set; }

        public DateTime? DateOfBirth { get; set; }

        [JsonConverter(typeof(JsonStringEnumConverter))]
        public Gender? Gender { get; set; }

        public string? Address { get; set; }

        public DateTime? JoinDate { get; set; }

        public int? VacationRemaining { get; set; }
        
        public string? CurrentWorkplace { get; set; }
        
        public EditEmployeeRequest_User? User {get; set;}
    }
    
    public class EditEmployeeRequest_User
	{
	    [EmailAddress(ErrorMessage = "メールアドレスの入力形式が誤っています")]
	    public string? Email { get; set; }
	    
	    [JsonConverter(typeof(JsonStringEnumConverter))]
        public UserRole? Role { get; set; }
	}
}