using Backend.Enums;
using System.Text.Json.Serialization;
using System.ComponentModel.DataAnnotations;

namespace Backend.DTOs
{
	public class UpdateMyInfoRequst
	{
	    public string FullName { get; set; }
	    public string Address { get; set; }
	    public string CurrentWorkplace { get; set; }
	    [JsonConverter(typeof(JsonStringEnumConverter))]
	    public Gender? Gender { get; set; }    
	    public UpdateMyInfoRequst_User? User {get; set;}
	}

	public class UpdateMyInfoRequst_User
	{
	    [EmailAddress(ErrorMessage = "メールアドレスの入力形式が誤っています")]
	    public string? Email { get; set; }
	}
}
