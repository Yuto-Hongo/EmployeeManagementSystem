using Backend.Data;
using Backend.DTOs;
using Backend.Models;
using Backend.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Backend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]

    public class RegistrationController : ControllerBase
    {
        // DB
        private readonly AppDbContext _db;
        

       // ログ出力
        private readonly ILogger<RegistrationController> _logger;

        public RegistrationController(ILogger<RegistrationController> logger, AppDbContext db)
        {
            _logger = logger;
            _db = db;
        }

        // 引数id(request.Id)に該当する従業員情報を出力する
        [HttpGet("{id}")]
        public async Task<IActionResult> GetEmployeeById(int id)
        {
            var employee = _db.Employees.FirstOrDefault(e => e.Id == id);

            // 該当idが存在しない場合NotFound()を返す
            if (employee == null)
            {
                _logger.LogError("従業員 ID={Id}が見つかりませんでした", id);
                return NotFound();

            }

            return Ok(employee);
        }

        // 新規登録メソッド
        // api/registration/signup
        [HttpPost("signup")]
        public async Task<IActionResult> Signup([FromBody] SignupRequest request)
        {
            // 入力データが不備・不正の場合弾く
            if (!ModelState.IsValid)
            {
                _logger.LogWarning("登録失敗：無効なリクエスト");
                return BadRequest(ModelState);
            }

            // 既存メールアドレス重複チェック
            if (await _db.Users.AnyAsync(u => u.Email == request.Email))
                return Conflict("このメールアドレスは既に登録されています");

            // Salt生成、パスワードHash化
            var salt = AuthService.GenerateSalt();
            var hashedPassword = AuthService.Hash(request.Password, salt);

            // Userテーブルレコード作成情報
            var user = new User
            {
                Email = request.Email,
                PasswordHash = hashedPassword,
                Salt = salt,
                Role = request.Role
            };

            // Userテーブルへレコード追加
            _db.Users.Add(user);
            await _db.SaveChangesAsync();
            
            var employee = new Employee
            {
            	UserId = user.Id
            };
            
            _db.Employees.Add(employee);
            await _db.SaveChangesAsync();

            // 追加した情報をログ出力
            _logger.LogInformation("新従業員を登録：{@User}", user);

            // レスポンス返却
            // return CreatedAtAction(nameof(GetEmployeeById), new { id = user.Id }, user);
            return Ok(new
            {
            	message = "ユーザー登録完了",
            	token = JwtTokenGenerator.GenerateToken(user)
            });
        }
    }
}