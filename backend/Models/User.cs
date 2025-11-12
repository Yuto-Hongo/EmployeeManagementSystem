using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using Backend.Enums;

namespace Backend.Models
{
    public class User
    {
        [Key]
        public int Id { get; set; }

        [EmailAddress]
        public string Email { get; set; }

        [StringLength(100, MinimumLength = 8)]
        [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d).+$")]
        public string PasswordHash { get; set; }

        [Required]
        public string Salt { get; set; }

        [JsonConverter(typeof(JsonStringEnumConverter))] //Jsono—Í‚É•¶š—ñ‚Ö•ÏŠ·
        public UserRole Role { get; set; } = UserRole.General;
    }
}
