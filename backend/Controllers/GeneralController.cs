using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Backend.Data;
using Backend.DTOs;
using Microsoft.EntityFrameworkCore;

namespace Backend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize(Roles = "General")]
    public class GeneralController : ControllerBase
    {
        // DB呼び出し
        private readonly AppDbContext _db;

        // ログ機能呼び出し
        private readonly ILogger<GeneralController> _logger;

        public GeneralController(AppDbContext db, ILogger<GeneralController> logger)
        {
            _db = db;
            _logger = logger;
        }

        // api/general
        // 一般従業員権限管理画面
        [HttpGet("info")]
        public async Task<IActionResult> GetMyEmployeeInfo()
        {
            var userIdClaim = User.FindFirst("userId")?.Value;

            // userIdClaimがnullの場合またはint型に変換出来ない場合にUnauthorizedを返す
            if (string.IsNullOrEmpty(userIdClaim) || !int.TryParse(userIdClaim, out int userId))
            {
                _logger.LogError("無効Id ID={userIdClaim}",
                   userIdClaim,
                   ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage).ToList()
                );

                return Unauthorized("ユーザーIDが無効です");
            }


            var employee = await _db.Employees.Include(e => e.User).FirstOrDefaultAsync(e => e.Id == userId);	 
            // var employee = await _db.Employees.FirstOrDefaultAsync(e => e.UserId == userId);

            // _employeesリスト内のIdにe.Idと同一要素が無い場合NotFoundを返す
            if (employee == null)
            {
                _logger.LogError("該当従業員なし ID={userId}",
                    userId,
                    ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage).ToList()
                );

                return NotFound("該当する従業員情報が見つかりません");
            }
                
            // _employeesリスト内のIdにe.Idと同一要素のみを(現在ログインしているユーザー情報)出力
            return Ok(employee);
        }

        // api/general/edit
        [HttpPut("edit")]
        public async Task<IActionResult> UpdateMyInfo([FromBody] UpdateMyInfoRequst request)
        {
            var userIdClaim = User.FindFirst("userId")?.Value;

            // userIdClaimがnullの場合またはint型に変換出来ない場合にUnauthorizedを返す
            if (string.IsNullOrEmpty(userIdClaim) || !int.TryParse(userIdClaim, out int userId))
            {
                _logger.LogError("無効Id ID={userIdClaim}",
                   userIdClaim,
                   ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage).ToList()
                );

                return Unauthorized("ユーザーIDが無効です");
            }

            var employee = await _db.Employees.Include(e => e.User).FirstOrDefaultAsync(e => e.UserId == userId);
            
            // _employeesリスト内のIdにe.Idと同一要素が無い場合NotFoundを返す
            if (employee == null)
            {
                _logger.LogError("該当従業員なし ID={userId}",
                    userId,
                    ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage).ToList()
                );

                return NotFound("該当する従業員情報が見つかりません");
            }

            // 項目に変更値が存在する場合は既存値を変更値に代入する
            // 変更が無い場合は既存値を引き継ぐ
            if (!string.IsNullOrWhiteSpace(request.FullName))
                employee.FullName = request.FullName;

            if (!string.IsNullOrEmpty(request.Address))
                employee.Address = request.Address;

            if (!string.IsNullOrEmpty(request.CurrentWorkplace))
                employee.CurrentWorkplace = request.CurrentWorkplace;

            if (request.User != null && !string.IsNullOrWhiteSpace(request.User.Email))
            {// 既存メールアドレス重複チェック
                if (await _db.Users.AnyAsync(u => u.Email == request.User.Email && u.Id != employee.UserId))
                {
                    return Conflict("このメールアドレスは既に登録されています");
                }
                else employee.User.Email = request.User.Email;
            }

            if (request.Gender.HasValue)
                employee.Gender = request.Gender.Value;

            // 更新情報保存
            await _db.SaveChangesAsync();

            var result = await _db.Employees
				.Include(e => e.User)
				.FirstOrDefaultAsync(e => e.Id == employee.Id);

            // 更新データログ出力
            _logger.LogInformation("ユーザー情報変更成功 ID={Id}", employee.Id);

            return Ok(new
            {
                message = "ユーザー情報を更新しました",
                result
            });
        }
    }
}