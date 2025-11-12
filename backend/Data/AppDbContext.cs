using Backend.Models;
using Microsoft.EntityFrameworkCore;

namespace Backend.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<Employee> Employees { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<AuditLog> AuditLogs { get; set; }

        public DbSet<Skill> Skills { get; set; }

        public DbSet<EmployeeSkill> EmployeeSkills { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // UserとEmployeeのマッピング
            modelBuilder.Entity<Employee>()
                .HasOne(e => e.User)
                .WithOne()
                .HasForeignKey<Employee>(e => e.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            // 多対多リレーション構成
            modelBuilder.Entity<EmployeeSkill>()
                .HasOne(es => es.Employee)
                .WithMany(e => e.EmployeeSkills)
                .HasForeignKey(es => es.EmployeeId);

            modelBuilder.Entity<EmployeeSkill>()
                .HasOne(es => es.Skill)
                .WithMany(s => s.EmployeeSkills)
                .HasForeignKey(es => es.SkillId);

            // Skill Name重複禁止
            modelBuilder.Entity<Skill>()
                .HasIndex(s => s.Name)
                .IsUnique();

            modelBuilder.Entity<Skill>().HasData(
                // === Backend Languages ===
                new Skill { Id = 1, Name = "C#", Category = SkillCategory.Backend, IconPath = "/assets/icons/csharp.svg" },
                new Skill { Id = 2, Name = "Java", Category = SkillCategory.Backend, IconPath = "/assets/icons/java.svg" },
                new Skill { Id = 3, Name = "Python", Category = SkillCategory.Backend, IconPath = "/assets/icons/python.svg" },
                new Skill { Id = 4, Name = "PHP", Category = SkillCategory.Backend, IconPath = "/assets/icons/php.svg" },
                new Skill { Id = 5, Name = "Go", Category = SkillCategory.Backend, IconPath = "/assets/icons/go.svg" },
                new Skill { Id = 6, Name = "Node.js", Category = SkillCategory.Backend, IconPath = "/assets/icons/nodejs.svg" },
                new Skill { Id = 7, Name = "Ruby", Category = SkillCategory.Backend, IconPath = "/assets/icons/ruby.svg" },

                // === Frontend Frameworks ===
                new Skill { Id = 8, Name = "HTML5", Category = SkillCategory.Frontend, IconPath = "/assets/icons/html5.svg" },
                new Skill { Id = 9, Name = "CSS3", Category = SkillCategory.Frontend, IconPath = "/assets/icons/css3.svg" },
                new Skill { Id = 10, Name = "JavaScript", Category = SkillCategory.Frontend, IconPath = "/assets/icons/javascript.svg" },
                new Skill { Id = 11, Name = "TypeScript", Category = SkillCategory.Frontend, IconPath = "/assets/icons/typescript.svg" },
                new Skill { Id = 12, Name = "Vue.js", Category = SkillCategory.Frontend, IconPath = "/assets/icons/vue.svg" },
                new Skill { Id = 13, Name = "React", Category = SkillCategory.Frontend, IconPath = "/assets/icons/react.svg" },
                new Skill { Id = 14, Name = "Angular", Category = SkillCategory.Frontend, IconPath = "/assets/icons/angular.svg" },
                new Skill { Id = 15, Name = "Blazor", Category = SkillCategory.Frontend, IconPath = "/assets/icons/blazor.svg" },

                // === Databases ===
                new Skill { Id = 16, Name = "SQL Server", Category = SkillCategory.Database, IconPath = "/assets/icons/mssql.svg" },
                new Skill { Id = 17, Name = "MySQL", Category = SkillCategory.Database, IconPath = "/assets/icons/mysql.svg" },
                new Skill { Id = 18, Name = "PostgreSQL", Category = SkillCategory.Database, IconPath = "/assets/icons/postgresql.svg" },
                new Skill { Id = 19, Name = "SQLite", Category = SkillCategory.Database, IconPath = "/assets/icons/sqlite.svg" },
                new Skill { Id = 20, Name = "Oracle", Category = SkillCategory.Database, IconPath = "/assets/icons/oracle.svg" },

                // === DevOps / Infrastructure ===
                new Skill { Id = 21, Name = "Docker", Category = SkillCategory.DevOps, IconPath = "/assets/icons/docker.svg" },
                new Skill { Id = 22, Name = "Kubernetes", Category = SkillCategory.DevOps, IconPath = "/assets/icons/kubernetes.svg" },
                new Skill { Id = 23, Name = "NGINX", Category = SkillCategory.DevOps, IconPath = "/assets/icons/nginx.svg" },
                new Skill { Id = 24, Name = "Apache", Category = SkillCategory.DevOps, IconPath = "/assets/icons/apache.svg" },

                // === Cloud Platforms ===
                new Skill { Id = 25, Name = "AWS", Category = SkillCategory.Cloud, IconPath = "/assets/icons/aws.svg" },
                new Skill { Id = 26, Name = "Azure", Category = SkillCategory.Cloud, IconPath = "/assets/icons/azure.svg" },
                new Skill { Id = 27, Name = "GCP", Category = SkillCategory.Cloud, IconPath = "/assets/icons/gcp.svg" },

                // === Version Control ===
                new Skill { Id = 28, Name = "Git", Category = SkillCategory.VersionControl, IconPath = "/assets/icons/git.svg" },
                new Skill { Id = 29, Name = "GitHub", Category = SkillCategory.VersionControl, IconPath = "/assets/icons/github.svg" },
                new Skill { Id = 30, Name = "GitLab", Category = SkillCategory.VersionControl, IconPath = "/assets/icons/gitlab.svg" },

                // === Tools ===
                new Skill { Id = 31, Name = "Visual Studio", Category = SkillCategory.Tool, IconPath = "/assets/icons/visualstudio.svg" },
                new Skill { Id = 32, Name = "VS Code", Category = SkillCategory.Tool, IconPath = "/assets/icons/vscode.svg" },
                new Skill { Id = 33, Name = "Postman", Category = SkillCategory.Tool, IconPath = "/assets/icons/postman.svg" },
                new Skill { Id = 34, Name = "Figma", Category = SkillCategory.Tool, IconPath = "/assets/icons/figma.svg" }
            );
        }
    }
}