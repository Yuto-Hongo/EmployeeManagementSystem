using Backend.Enums;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Backend.DTOs
{
    public class CreateEmployeeRequest
    {
        [Required(ErrorMessage = "氏名を入力してください")]
        public string FullName { get; set; }

        [Required(ErrorMessage = "生年月日を入力してください")]
        [DataType(DataType.Date)]
        public DateTime DateOfBirth { get; set; }

        [Required]
        [JsonConverter(typeof(JsonStringEnumConverter))]
        public Gender? Gender { get; set; }

        [Required(ErrorMessage = "住所を入力してください")]
        public string Address { get; set; }

        [Required(ErrorMessage = "入社日を入力してください")]
        [DataType(DataType.Date)]
        public DateTime JoinDate { get; set; }

        [Required(ErrorMessage =    "勤務先を入力してください")]
        public string CurrentWorkplace { get; set; }
        
        public CreateEmployeeRequest_User User {get; set;}
    }
    
    	public class CreateEmployeeRequest_User
	{
	    [EmailAddress(ErrorMessage = "メールアドレスの入力形式が誤っています")]
	    public string? Email { get; set; }
	    
	    public UserRole? Role { get; set; } = UserRole.General;
	}
}