using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace EmployeeManagement.Api.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AuditLogs",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    ActionUserId = table.Column<int>(type: "INTEGER", nullable: false),
                    Action = table.Column<string>(type: "TEXT", nullable: false),
                    EntityName = table.Column<string>(type: "TEXT", nullable: false),
                    TargetId = table.Column<int>(type: "INTEGER", nullable: false),
                    Timestamp = table.Column<DateTime>(type: "TEXT", nullable: false),
                    Details = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AuditLogs", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Skills",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(type: "TEXT", nullable: false),
                    Category = table.Column<int>(type: "INTEGER", nullable: false),
                    IconPath = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Skills", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Email = table.Column<string>(type: "TEXT", nullable: false),
                    PasswordHash = table.Column<string>(type: "TEXT", maxLength: 100, nullable: false),
                    Salt = table.Column<string>(type: "TEXT", nullable: false),
                    Role = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Employees",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    UserId = table.Column<int>(type: "INTEGER", nullable: false),
                    FullName = table.Column<string>(type: "TEXT", nullable: true),
                    DateOfBirth = table.Column<DateTime>(type: "TEXT", nullable: true),
                    Gender = table.Column<int>(type: "INTEGER", nullable: true),
                    Address = table.Column<string>(type: "TEXT", nullable: true),
                    JoinDate = table.Column<DateTime>(type: "TEXT", nullable: true),
                    VacationRemaining = table.Column<int>(type: "INTEGER", nullable: true),
                    CurrentWorkplace = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Employees", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Employees_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "EmployeeSkills",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    EmployeeId = table.Column<int>(type: "INTEGER", nullable: false),
                    SkillId = table.Column<int>(type: "INTEGER", nullable: false),
                    Level = table.Column<int>(type: "INTEGER", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EmployeeSkills", x => x.Id);
                    table.ForeignKey(
                        name: "FK_EmployeeSkills_Employees_EmployeeId",
                        column: x => x.EmployeeId,
                        principalTable: "Employees",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_EmployeeSkills_Skills_SkillId",
                        column: x => x.SkillId,
                        principalTable: "Skills",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "Skills",
                columns: new[] { "Id", "Category", "IconPath", "Name" },
                values: new object[,]
                {
                    { 1, 1, "/assets/icons/csharp.svg", "C#" },
                    { 2, 1, "/assets/icons/java.svg", "Java" },
                    { 3, 1, "/assets/icons/python.svg", "Python" },
                    { 4, 1, "/assets/icons/php.svg", "PHP" },
                    { 5, 1, "/assets/icons/go.svg", "Go" },
                    { 6, 1, "/assets/icons/nodejs.svg", "Node.js" },
                    { 7, 1, "/assets/icons/ruby.svg", "Ruby" },
                    { 8, 2, "/assets/icons/html5.svg", "HTML5" },
                    { 9, 2, "/assets/icons/css3.svg", "CSS3" },
                    { 10, 2, "/assets/icons/javascript.svg", "JavaScript" },
                    { 11, 2, "/assets/icons/typescript.svg", "TypeScript" },
                    { 12, 2, "/assets/icons/vue.svg", "Vue.js" },
                    { 13, 2, "/assets/icons/react.svg", "React" },
                    { 14, 2, "/assets/icons/angular.svg", "Angular" },
                    { 15, 2, "/assets/icons/blazor.svg", "Blazor" },
                    { 16, 3, "/assets/icons/mssql.svg", "SQL Server" },
                    { 17, 3, "/assets/icons/mysql.svg", "MySQL" },
                    { 18, 3, "/assets/icons/postgresql.svg", "PostgreSQL" },
                    { 19, 3, "/assets/icons/sqlite.svg", "SQLite" },
                    { 20, 3, "/assets/icons/oracle.svg", "Oracle" },
                    { 21, 4, "/assets/icons/docker.svg", "Docker" },
                    { 22, 4, "/assets/icons/kubernetes.svg", "Kubernetes" },
                    { 23, 4, "/assets/icons/nginx.svg", "NGINX" },
                    { 24, 4, "/assets/icons/apache.svg", "Apache" },
                    { 25, 6, "/assets/icons/aws.svg", "AWS" },
                    { 26, 6, "/assets/icons/azure.svg", "Azure" },
                    { 27, 6, "/assets/icons/gcp.svg", "GCP" },
                    { 28, 5, "/assets/icons/git.svg", "Git" },
                    { 29, 5, "/assets/icons/github.svg", "GitHub" },
                    { 30, 5, "/assets/icons/gitlab.svg", "GitLab" },
                    { 31, 7, "/assets/icons/visualstudio.svg", "Visual Studio" },
                    { 32, 7, "/assets/icons/vscode.svg", "VS Code" },
                    { 33, 7, "/assets/icons/postman.svg", "Postman" },
                    { 34, 7, "/assets/icons/figma.svg", "Figma" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Employees_UserId",
                table: "Employees",
                column: "UserId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_EmployeeSkills_EmployeeId",
                table: "EmployeeSkills",
                column: "EmployeeId");

            migrationBuilder.CreateIndex(
                name: "IX_EmployeeSkills_SkillId",
                table: "EmployeeSkills",
                column: "SkillId");

            migrationBuilder.CreateIndex(
                name: "IX_Skills_Name",
                table: "Skills",
                column: "Name",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AuditLogs");

            migrationBuilder.DropTable(
                name: "EmployeeSkills");

            migrationBuilder.DropTable(
                name: "Employees");

            migrationBuilder.DropTable(
                name: "Skills");

            migrationBuilder.DropTable(
                name: "Users");
        }
    }
}
