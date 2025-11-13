using System.IdentityModel.Tokens.Jwt;
using Backend.Models;
using System.Security.Claims;
using Microsoft.IdentityModel.Tokens;
using Microsoft.Extensions.Configuration;

public class JwtTokenGenerator
{
    // 定数SecreKey宣言
    private readonly IConfiguration _configuration;

    public JwtTokenGenerator(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    // ログイン成功時に呼び出されるGenerateTokenメソッド
    public string GenerateToken(User user)
    {
        // トークンに埋め込むユーザー情報(UserId, Role)を配列に格納
        var claims = new[]
        {
            new Claim("userId", user.Id.ToString()),
            new Claim("Roles", user.Role.ToString())
        };

        var secretKey = _configuration["JWT:Key"];
        var issuer = _configuration["JWT:Issuer"];
        var audience = _configuration["JWT:Audience"];

        var key = new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(secretKey));
        var creads = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        // トークン本体情報を生成
        var token = new JwtSecurityToken(
            issuer: issuer, //発行者
            audience: audience, //対象者
            claims: claims, //ユーザー情報
            expires: DateTime.UtcNow.AddHours(1), //有効期限
            signingCredentials: creads //署名情報
            );

        // token内容を基にJWT文字列を生成
        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}