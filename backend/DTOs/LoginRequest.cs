using System.ComponentModel.DataAnnotations;

namespace Backend.DTOs
{
    public class LoginRequest
    {
        [Required(ErrorMessage = "メールアドレスを入力してください")]
        [EmailAddress(ErrorMessage = "メールアドレスの入力形式が誤っています")]
        public string Email { get; set; }

        [Required(ErrorMessage = "パスワードを入力してください")]
        [StringLength(100, MinimumLength = 8, ErrorMessage = "パスワードは8文字以上で入力してください")]
        [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d).+$", ErrorMessage = "パスワードは英大文字・小文字・数字を含む必要があります")]
        public string Password { get; set; }
    }
}