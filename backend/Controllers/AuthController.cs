using Backend.Data;
using Backend.DTOs;
using Backend.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Backend.Controllers
{
    // コントローラーをのルートパス指定
    [ApiController]
    [Route("api/[controller]")]

    public class AuthController : ControllerBase
    {
        // 読み取り専用でAuthServiceを呼び出す
        private readonly AuthService _authService;

        // ログ
        private readonly ILogger<AuthController> _logger;

        // DB
        private readonly AppDbContext _db;

        private readonly JwtTokenGenerator _tokenGenerator;

        public AuthController(ILogger<AuthController> logger, AppDbContext db, AuthService authService, JwtTokenGenerator tokenGenerator)
        {
            _logger = logger;
            _db = db;
            _authService = authService;
            _tokenGenerator = tokenGenerator;
        }

        // ログインメソッド
        // api/auth/login
        [HttpPost("login")] 
        public async Task<IActionResult> Login([FromBody] LoginRequest request)
        {
            // DB内に引数（ユーザー情報）があるか確認
            var loginUser = await _authService.ValidateUserAsync(request.Email, request.Password);

            // 一致要素無しの場合(401)、Unauthorizedエラー+メッセージ
            if (loginUser == null) 
            {
                _logger.LogWarning($"ログイン認証エラー Email : {request.Email}, Password:{request.Password}",
                    request.Email, request.Password,
                    ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage).ToList()
                 );
                return Unauthorized("メールアドレスまたはパスワードに誤りがあります");
            }

            // 一致要素有りの場合(200) 、 トークン作成+所持Roleを返す
            //var token = JwtTokenGenerator.GenerateToken(loginUser);
            var token = _tokenGenerator.GenerateToken(loginUser);

            _logger.LogInformation($"ログイン処理成功 : UserId : {loginUser.Id}", loginUser.Id);
            return Ok(new LoginResponse
            {
                Token = token,
                Role = loginUser.Role
            });
        }

        // パスワード変更メソッド
        // api/auth/change-password
        [Authorize]
        [HttpPut("change-password")]
        public async Task<IActionResult> ChangePassword([FromBody] ChangePasswordRequest request)
        {
            // バリデーションエラーが発生した際にログ取得
            if (!ModelState.IsValid)
            {
                _logger.LogWarning("登録失敗：無効リクエスト");
                return BadRequest(ModelState);
            }

            // 認証ユーザーのIDを取得
            var userId = int.Parse(User.FindFirst("userId")?.Value);

            // ユーザー情報を取得
            var user = _db.Users.FirstOrDefault(u => u.Id == userId);

            var salt = AuthService.GenerateSalt();
            var requestCurrentPass = AuthService.Hash(request.CurrentPassword, user.Salt);

            // 一致Idが存在しない場合の処理
            if (user == null) 
            {
                _logger.LogError($"従業員認証エラー Id : {userId}",
                    userId,
                    ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage).ToList()
                );
                return Unauthorized("ユーザが見つかりません");
            }

            if (requestCurrentPass != user.PasswordHash)
            {
                _logger.LogError($"既存のパスワードが一致しません, UserId : {userId}",
                    userId,
                    ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage).ToList()
                );
                return Unauthorized("既存のパスワードが一致しません");
            }

            // パスワード更新
            user.PasswordHash = AuthService.Hash(request.NewPassword, salt);
            user.Salt = salt;

            await _db.SaveChangesAsync();

            _logger.LogInformation($"パスワード変更処理成功 UserId : {userId}", userId, user.PasswordHash);
            return Ok(new { message = "パスワードを変更しました", user.PasswordHash });
        }

        // 認証ユーザー確認
        // api/auth/profile
        [HttpGet("profile")]
        [Authorize]
        public IActionResult GetProfile()
        {
            var userId = User.FindFirst("userId")?.Value;
            var role = User.FindFirst("Roles")?.Value;

            if (userId == null)
            {
                _logger.LogError("未承認ユーザー");
                return NotFound("トークン認証がされていないユーザーです");
            }

            return Ok(new
            {
                Message = "トークンによって保護されたデータです",
                UserId = userId,
                Role = role
            });
        }
    }
}
   