using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Backend.Models {
    public enum SkillCategory 
    {
        Backend = 1,
        Frontend = 2,
        Database = 3,
        DevOps = 4,
        VersionControl = 5,
        Cloud = 6,
        Tool = 7
    }

    public class Skill
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public string Name { get; set; } = string.Empty;

        public SkillCategory Category { get; set; }

        // アイコンURL
        public string? IconPath { get; set; }

        // EmployeeSkillとのリレーション
        public ICollection<EmployeeSkill> EmployeeSkills { get; set; } = new List<EmployeeSkill>();
    }
}