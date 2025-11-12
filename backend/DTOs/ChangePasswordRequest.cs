using System.ComponentModel.DataAnnotations;

namespace Backend.DTOs
{
    public class ChangePasswordRequest
    {
        [Required(ErrorMessage = "現在のパスワードを入力してください")]
        public string CurrentPassword { get; set; }

        [Required(ErrorMessage = "新しいパスワードを入力してください")]
        [StringLength(100, MinimumLength = 8, ErrorMessage = "パスワードは8文字以上で入力してください")]
        [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d).+$", ErrorMessage = "パスワードは英大文字・小文字・数字を含む必要があります")]
        public string NewPassword { get; set; }

        [Required(ErrorMessage = "新しいパスワード(確認)を入力してください")]
        [Compare("NewPassword", ErrorMessage = "新しいパスワードとパスワード(確認)が一致しません")]
        public string ConfirmPassword { get; set; }
    }
}