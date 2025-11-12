using Microsoft.AspNetCore.Mvc;
using Backend.Models;
using Microsoft.AspNetCore.Authorization;
using Backend.DTOs;
using Backend.Services;
using Backend.Data;
using Backend.Enums;
using Microsoft.EntityFrameworkCore;

namespace Backend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize(Roles = "Admin")]
    public class AdminController : ControllerBase
    {
        // DB呼び出し
        private readonly AppDbContext _db;

        // ログ機能呼び出し
        private readonly ILogger<AdminController> _logger;

        public AdminController(ILogger<AdminController> logger, AppDbContext db)
        {
            _logger = logger;
            _db = db;
        }

        // api/admin/employees
        // 従業員情報全容出力メソッド
        [HttpGet("employees")]
        public async Task<IActionResult> GetAllEmployee()
        {
            var employees = await _db.Employees.Include(e => e.User).ToListAsync();
            return Ok(employees);
        }

        // api/admin/users
        // *検証用 Usersテーブル全件出力
        [HttpGet("users")]
        public async Task<IActionResult> GetAllUser()
        {
            var users = await _db.Users.ToListAsync();
            return Ok(users);
        }

        // api/admin/edit/{id}
        // ユーザー情報変更メソッド
        [HttpPut("edit/{id}")]
        public async Task<IActionResult> UpdateEmployee(int id, [FromBody] EditEmployeeRequest request)
        {
            // バリデーションエラーが発生した際にログ取得
            if (!ModelState.IsValid)
            {
                _logger.LogWarning("モデルエラー：{@ModelState}", ModelState);
                return BadRequest(ModelState);
            }

            // リクエストで受け取った値がdbに存在するかチェック
            var employee = await _db.Employees.Include(e => e.User).FirstOrDefaultAsync(e => e.Id == id);

            // 一致する要素が存在しない場合はNotFoundを返す
            if (employee == null)
            {
                _logger.LogError("従業員 ID={Id}が見つかりませんでした",
                    id,
                    ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage).ToList()
                );
                return NotFound("対象のユーザーが存在しません");
            }

            // 項目に変更値が存在する場合は既存値を変更値に代入する
            // 変更が無い場合は既存値を引き継ぐ
            if (!string.IsNullOrWhiteSpace(request.FullName))
                employee.FullName = request.FullName;

            if (request.DateOfBirth.HasValue)
                employee.DateOfBirth = request.DateOfBirth.Value;

            if (request.Gender.HasValue)
                employee.Gender = request.Gender.Value;

            if (!string.IsNullOrEmpty(request.Address))
                employee.Address = request.Address;

            if (request.JoinDate.HasValue)
                employee.JoinDate = request.JoinDate.Value;

            if (request.VacationRemaining.HasValue)
                employee.VacationRemaining = request.VacationRemaining.Value;

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


            if (request.User.Role.HasValue)
                employee.User.Role = request.User.Role.Value;

            // 更新情報保存
            await _db.SaveChangesAsync();

            // 更新データログ出力
            _logger.LogInformation("ユーザー情報変更成功 ID={Id}", employee.Id);

            // 本メソッドを実行するuserIdを取得
            var userIdClaim = User.FindFirst("userId")?.Value;
            int actionUserId = int.Parse(userIdClaim);

            // 監査ログ
            var auditLog = new AuditLog
            {
                ActionUserId = actionUserId,
                Action = "Update",
                EntityName = "Employee and User",
                TargetId = employee.Id,
                Timestamp = DateTime.UtcNow,
                Details = $"Updated employee {employee.FullName}"
            };
            _db.AuditLogs.Add(auditLog);

            await _db.SaveChangesAsync();

            return Ok(new
            {
                message = "ユーザー情報を更新しました",
                employee
            });
        }

        // フィルター・ソート機能
        // api/admin/search
        [HttpGet("search")]
        public async Task<IActionResult> SearchEmployee(
            [FromQuery] string? name,
            [FromQuery] string? workplace,
            [FromQuery] DateTime? joinDateFrom,
            [FromQuery] DateTime? joinDateTo,
            [FromQuery] List<int>? skillIds,
            [FromQuery] string mode = "and", // デフォルトモード：AND
            [FromQuery] int page = 1,
            [FromQuery] int pageSize = 10,
            [FromQuery] string sortBy = "Id",
            [FromQuery] string sortOrder = "asc"
            )
        {
            // フィルター処理
            var query = _db.Employees
                .Include(e => e.User)
                .Include(e => e.EmployeeSkills)
                .ThenInclude(es => es.Skill)
                .AsQueryable();

            // AND条件
            if (mode.ToLower() == "and")
            {
                if (!string.IsNullOrEmpty(name))
                    query = query.Where(e => e.FullName.Contains(name));

                if (!string.IsNullOrEmpty(workplace))
                    query = query.Where(e => e.CurrentWorkplace == workplace);

                if (joinDateFrom.HasValue)
                    query = query.Where(e => e.JoinDate >= joinDateFrom.Value);

                if (joinDateTo.HasValue)
                    query = query.Where(e => e.JoinDate <= joinDateTo.Value);

                if (skillIds != null && skillIds.Any())
                {
                    query = query.Where(e =>
                        skillIds.All(skillId =>
                        e.EmployeeSkills.Any(es => es.SkillId == skillId)
                        )
                    );
                }
            }
            // OR条件
            else if (mode.ToLower() == "or")
            {
                query = query.Where(e =>
                    (!string.IsNullOrEmpty(name) && e.FullName.Contains(name)) ||
                    (!string.IsNullOrEmpty(workplace) && e.CurrentWorkplace == workplace) ||
                    (joinDateFrom.HasValue && e.JoinDate >= joinDateFrom.Value) ||
                    (joinDateTo.HasValue && e.JoinDate <= joinDateTo.Value) ||
                    (skillIds != null && e.EmployeeSkills.Any(es => skillIds.Contains(es.SkillId)))
                    );
            }
            else
            {
                return BadRequest("modeは'and'または'or'を指定して下さい");
            }

            // ソート処理
            query = sortBy.ToLower() switch
            {
                "name" => sortOrder.ToLower() == "desc"
                ? query.OrderByDescending(e => e.FullName)
                : query.OrderBy(e => e.FullName),

                "joindate" => sortOrder.ToLower() == "desc"
                ? query.OrderByDescending(e => e.JoinDate)
                : query.OrderBy(e => e.JoinDate),

                "workplace" => sortOrder.ToLower() == "desc"
                ? query.OrderByDescending(e => e.CurrentWorkplace)
                : query.OrderBy(e => e.CurrentWorkplace),

                _ => sortOrder.ToLower() == "desc"
                ? query.OrderByDescending(e => e.Id)
                : query.OrderBy(e => e.Id),
            };

            // ページング処理
            var totalCount = await query.CountAsync();
            var skip = (page - 1) * pageSize;

            var result = await query.Skip(skip).Take(pageSize).ToListAsync();

            if (!result.Any())
                return NotFound("条件に一致するデータはありません");

            var employeeDtos = result.Select(e => new SelectedEmployeeInfoResponse
            {
                Id = e.Id,
                FullName = e.FullName,
                DateOfBirth = e.DateOfBirth ?? default(DateTime),
                Gender = e.Gender ?? default(Backend.Enums.Gender),
                Address = e.Address,
                JoinDate = e.JoinDate ?? default(DateTime),
                CurrentWorkplace = e.CurrentWorkplace,
                User = e.User == null ? null : new UserInfoDto
                {
                    Email = e.User.Email,
                    Role = e.User.Role
                },
                Skills = e.EmployeeSkills.Select(es => new SkillInfoDto
                {
                    Name = es.Skill.Name,
                    Category = es.Skill.Category.ToString(),
                    IconPath = es.Skill.IconPath
                }).ToList()
            }).ToList();

            // レスポンス形式
            return Ok(new
            {
                data = employeeDtos,
                totalCount,
                currentPage = page,
                pageSize,
                totalPages = (int)Math.Ceiling((double)totalCount / pageSize)
            });
        }

        // api/admin/add-employee
        // ユーザー情報を作成するメソッド(管理者管理画面から新規登録できる機能)
        [HttpPost("add-employee")]
        public async Task<IActionResult> CreateEmployee([FromBody] CreateEmployeeRequest request)
        {
            // バリデーションエラーが発生した際にログ取得
            if (!ModelState.IsValid)
            {
                _logger.LogWarning("モデルエラー：{@ModelState}", ModelState);
                return BadRequest(ModelState);
            }

            // 既存メールアドレス重複チェック
            if (await _db.Users.AnyAsync(u => u.Email == request.User.Email))
                return Conflict("このメールアドレスは既に登録されています");

            // Salt生成、パスワードHash化
            var salt = AuthService.GenerateSalt();
            var defaultPassword = AuthService.Hash("Default123!", salt);

            // Userテーブルレコード作成情報
            var user = new User
            {
                Email = request.User.Email,
                PasswordHash = defaultPassword,
                Salt = salt,
                Role = request.User.Role ?? UserRole.General
            };

            // Userテーブルへレコード追加
            _db.Users.Add(user);
            await _db.SaveChangesAsync();

            // Employeeテーブルレコード作成情報
            var employee = new Employee
            {
                UserId = user.Id,
                FullName = request.FullName,
                DateOfBirth = request.DateOfBirth,
                Gender = request.Gender,
                Address = request.Address,
                JoinDate = request.JoinDate,
                CurrentWorkplace = request.CurrentWorkplace,
            };

            // Employeeテーブルへレコード追加
            _db.Employees.Add(employee);
            await _db.SaveChangesAsync();

            // 追加した情報をログ出力
            _logger.LogInformation("新従業員を登録：{@Employee}", employee);

            // 本メソッドを実行するuserIdを取得
            var userIdClaim = User.FindFirst("userId")?.Value;
            int actionUserId = int.Parse(userIdClaim);

            // 監査ログ
            var auditLog = new AuditLog
            {
                ActionUserId = actionUserId,
                Action = "AddEmplpoyee",
                EntityName = "Employee and User",
                TargetId = employee.Id,
                Timestamp = DateTime.UtcNow,
                Details = $"Added employee {employee.FullName}"
            };
            _db.AuditLogs.Add(auditLog);

            await _db.SaveChangesAsync();

            var result = await _db.Employees
                .Include(e => e.User)
                .FirstOrDefaultAsync(e => e.Id == employee.Id);

            // レスポンス返却
            return CreatedAtAction(nameof(GetEmployeeById), new { id = employee.Id }, result);
        }

        // api/admin/{id}
        // 指定したユーザー情報を出力
        [HttpGet("{id}")]
        public async Task<IActionResult> GetEmployeeById(int id)
        {
            var employee = await _db.Employees
                .Include(e => e.User)
                .Include(e => e.EmployeeSkills)
                    .ThenInclude(es => es.Skill)
                .FirstOrDefaultAsync(e => e.Id == id);

            // 該当idが存在しない場合NotFound()を返す
            if (employee == null)
            {
                _logger.LogError("従業員 ID={Id}が見つかりませんでした", id);
                return NotFound();
            }

            var response =  new SelectedEmployeeInfoResponse
            {
                Id = employee.Id,
                FullName = employee.FullName,
                DateOfBirth = employee.DateOfBirth ?? default(DateTime),
                Gender = employee.Gender ?? default(Backend.Enums.Gender),
                Address = employee.Address,
                JoinDate = employee.JoinDate ?? default(DateTime),
                CurrentWorkplace = employee.CurrentWorkplace,
                User = employee.User == null ? null : new UserInfoDto
                {
                    Email = employee.User.Email,
                    Role = employee.User.Role
                },
                Skills = employee.EmployeeSkills
            .Select(es => new SkillInfoDto
            {
                Name = es.Skill.Name,
                Category = es.Skill.Category.ToString(),
                IconPath = es.Skill.IconPath
            })
            .ToList()
            };

            return Ok(response);
        }

        // api/admin/{id}
        // 指定したユーザー削除
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteEmployee(int id)
        {
            // 削除対象 (Users, Employeesテーブル)
            var employee = await _db.Employees.FindAsync(id);
            var user = await _db.Users.FindAsync(id);

            if (user == null)
            {
                _logger.LogWarning("削除対象が存在しません EmployeeId = {Id}", id);
                NotFound("削除対象の従業員が存在しません");
            }

            _db.Users.Remove(user);
            await _db.SaveChangesAsync();

            // 本メソッドを実行するuserIdを取得
            var userIdClaim = User.FindFirst("userId")?.Value;
            int actionUserId = int.Parse(userIdClaim);

            // 監査ログ
            var auditLog = new AuditLog
            {
                ActionUserId = actionUserId,
                Action = "Delete",
                EntityName = "Employee and User",
                TargetId = employee.Id,
                Timestamp = DateTime.UtcNow,
                Details = $"Deleted employee {employee.FullName}"
            };
            _db.AuditLogs.Add(auditLog);
            await _db.SaveChangesAsync();

            _logger.LogInformation($"削除完了 Id = {user.Id}, FullName = {employee.FullName}", id);

            return NoContent();
        }

        // 管理者権限による指定ユーザーのパスワード初期化
        // /api/admin/reset-password/{id}
        [HttpPut("reset-password/{id}")]
        public async Task<IActionResult> ResetPassword(int id)
        {
            var user = await _db.Users.FirstOrDefaultAsync(u => u.Id == id);
            if (user == null) return NotFound("対象のユーザーが存在しません");

            // 新しいSaltとハッシュを生成
            var salt = AuthService.GenerateSalt();
            var defaultPassword = "Default123!";
            var hashed = AuthService.Hash(defaultPassword, salt);

            user.PasswordHash = hashed;
            user.Salt = salt;

            // 本メソッドを実行するuserIdを取得
            var userIdClaim = User.FindFirst("userId")?.Value;
            int actionUserId = int.Parse(userIdClaim);

            // 監査ログ
            var auditLog = new AuditLog
            {
                ActionUserId = actionUserId,
                Action = "ResetPassword",
                EntityName = "User",
                TargetId = user.Id,
                Timestamp = DateTime.UtcNow,
                Details = $"Admin reset password for userId={user.Id}"
            };
            _db.AuditLogs.Add(auditLog);

            await _db.SaveChangesAsync();

            _logger.LogInformation("Admin:{AdminId} reset password for UserId:{UserId}",
               auditLog.ActionUserId, user.Id);

            return Ok(new { message = "パスワードを初期化しました。" });
        }


        // api/admin/audit-logs
        // 監査ログ出力(直近100件)
        [HttpGet("audit-logs")]
        public async Task<IActionResult> GetAuditLogs()
        {
            var logs = await _db.AuditLogs
                .OrderByDescending(l => l.Timestamp)
                .Take(100)
                .ToListAsync();

            if (logs == null)
            {
                _logger.LogWarning("ログが存在しません");
                return NotFound("ログが存在しません");
            }

            return Ok(logs);
        }
    }
}


