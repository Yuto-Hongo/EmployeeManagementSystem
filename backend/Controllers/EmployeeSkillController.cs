using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Backend.Data;
using Backend.Models;
using Backend.DTOs;

namespace Backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmployeeSkillController : ControllerBase
    {
        private readonly AppDbContext _db;

        public EmployeeSkillController(AppDbContext db)
        {
            _db = db;
        }

        [HttpGet("skills")]
        public async Task<IActionResult> GetAllSkills()
        {
            var skills = await _db.Skills.ToListAsync();
            return Ok(skills);
        }

        // 指定従業員のスキル一覧取得
        [HttpGet("{employeeId}")]
        [Authorize]
        public async Task<IActionResult> GetSkillByEmployee(int employeeId)
        {
            var skills = await _db.EmployeeSkills
                .Include(es => es.Skill)
                .Where(es => es.EmployeeId == employeeId)
                .Select(es => new
                {
                    es.Skill.Id,
                    es.Skill.Name,
                    es.Skill.Category,
                    es.Skill.IconPath
                })
                .ToListAsync();

            return Ok(skills);
        }

        // スキル割り当て
        [HttpPost("assign")]
        [Authorize]
        public async Task<IActionResult> AssignSkills([FromBody] AssignSkillRequest request)
        {
            var employee = await _db.Employees.FindAsync(request.EmployeeId);
            if (employee == null) return NotFound("指定した従業員が存在しません");

            var existing = await _db.EmployeeSkills
                .Where(es => es.EmployeeId == request.EmployeeId)
                .Select(es => es.SkillId)
                .ToArrayAsync();

            var newSkills = request.SkillIds.Except(existing).ToList();
            if (!newSkills.Any())
                return BadRequest("既に割り当て済のスキルです");

            foreach (var skillId in newSkills)
            {
                _db.EmployeeSkills.Add(new EmployeeSkill
                {
                    EmployeeId = request.EmployeeId,
                    SkillId = skillId
                });
            }

            await _db.SaveChangesAsync();
            return Ok(new { message = "スキルを割り当てました" });
        }

        // スキル解除
        [HttpDelete("remove")]
        [Authorize]
        public async Task<IActionResult> RemoveSkill([FromBody] RemoveSkillRequest request)
        {
            var target = await _db.EmployeeSkills
                .FirstOrDefaultAsync(es => es.EmployeeId == request.EmployeeId && es.SkillId == request.SkillId);

            if (target == null)
                return NotFound("指定されたスキル割り当てが存在しません");

            _db.EmployeeSkills.Remove(target);
            await _db.SaveChangesAsync();
            return Ok(new { message = "スキルを解除しました" });
        }

        // 指定スキルを持つ従業員検索
        [HttpGet("search-by-skill")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> SearchEmployeeBySkills([FromQuery] List<int> skillIds, [FromQuery] string mode = "and")
        {
            if (!skillIds.Any()) return BadRequest("スキルIDが指定されていません");

            var query = _db.Employees
                .Include(e => e.EmployeeSkills)
                .ThenInclude(es => es.Skill)
                .AsQueryable();

            if (mode.ToLower() == "and")
            {
                foreach (var skillId in skillIds)
                {
                    query = query.Where(e => e.EmployeeSkills.Any(es => es.SkillId == skillId));
                }
            }
            else
            {
                query = query.Where(e => e.EmployeeSkills.Any(es => skillIds.Contains(es.SkillId)));
            }

            var employees = await query
                .Select(e => new
                {
                    e.Id,
                    e.FullName,
                    e.CurrentWorkplace,
                    Skills = e.EmployeeSkills.Select(es => es.Skill.Name).ToList()
                })
                .ToListAsync();

            return Ok(employees);
        }
    }
}