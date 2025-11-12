using System.Security.Cryptography;
using System.Text;
using Backend.Models;
using Backend.Data;
using Microsoft.EntityFrameworkCore;

namespace Backend.Services    
{
    public class AuthService
    {
        private readonly AppDbContext _db;

        public AuthService(AppDbContext db)
        {
            _db = db;
        }

        // 引数emilがUsersテーブルのEmail要素と一致した場合、一致したユーザー情報をパスワードハッシュ化して返す
        public async Task<User?> ValidateUserAsync(string email, string password)
        {
            var user = await _db.Users.FirstOrDefaultAsync(u => u.Email == email);

            if (user == null) return null;

            var hashedInputed = Hash(password, user.Salt);
            
            if(user.PasswordHash == hashedInputed)
                return user;

            return null;
        }

        // 16byteのSalt値出力
        public static string GenerateSalt()
        {
            var random = new byte[16];
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(random);
            }
            return Convert.ToBase64String(random);
        }

        // パスワードハッシュ化
        public static string Hash(string password, string salt)
        {
            using var sha256 = SHA256.Create(); //SHA-256（暗号学的ハッシュ関数）のインスタンスを作成
            var combined = Encoding.UTF8.GetBytes(password + salt);//引数 + saltをUTF-8のバイト列に変換
            var hash = sha256.ComputeHash(combined); //SHA-256を適用する
            return Convert.ToBase64String(hash); //Base64形式の文字列へ変換して返す
        }
    }
}
