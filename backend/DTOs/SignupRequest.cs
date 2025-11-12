using Backend.Enums;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Backend.DTOs
{
    public class SignupRequest
    {
        [Required(ErrorMessage = "メールアドレスを入力してください")]
        [EmailAddress(ErrorMessage = "メールアドレスの入力形式が誤っています")]
        public string Email { get; set; }

        [Required(ErrorMessage = "パスワードを入力してください")]
        [StringLength(100, MinimumLength = 8, ErrorMessage = "パスワードは8文字以上で入力してください")]
        [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d).+$", ErrorMessage = "パスワードは英大文字・小文字・数字を含む必要があります")]
        public string Password { get; set; }
        
        [Required(ErrorMessage = "パスワード(確認)を入力してください")]
        [Compare("Password", ErrorMessage = "パスワードとパスワード(確認)が一致しません")]
        public string ConfirmPassword { get; set; }

        [JsonConverter(typeof(JsonStringEnumConverter))]
        public UserRole Role { get; set; } = UserRole.General;
    }
}