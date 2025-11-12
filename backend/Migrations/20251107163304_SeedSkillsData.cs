using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace backend.Migrations
{
    /// <inheritdoc />
    public partial class SeedSkillsData : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
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
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Skills",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Skills",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Skills",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "Skills",
                keyColumn: "Id",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "Skills",
                keyColumn: "Id",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "Skills",
                keyColumn: "Id",
                keyValue: 6);

            migrationBuilder.DeleteData(
                table: "Skills",
                keyColumn: "Id",
                keyValue: 7);

            migrationBuilder.DeleteData(
                table: "Skills",
                keyColumn: "Id",
                keyValue: 8);

            migrationBuilder.DeleteData(
                table: "Skills",
                keyColumn: "Id",
                keyValue: 9);

            migrationBuilder.DeleteData(
                table: "Skills",
                keyColumn: "Id",
                keyValue: 10);

            migrationBuilder.DeleteData(
                table: "Skills",
                keyColumn: "Id",
                keyValue: 11);

            migrationBuilder.DeleteData(
                table: "Skills",
                keyColumn: "Id",
                keyValue: 12);

            migrationBuilder.DeleteData(
                table: "Skills",
                keyColumn: "Id",
                keyValue: 13);

            migrationBuilder.DeleteData(
                table: "Skills",
                keyColumn: "Id",
                keyValue: 14);

            migrationBuilder.DeleteData(
                table: "Skills",
                keyColumn: "Id",
                keyValue: 15);

            migrationBuilder.DeleteData(
                table: "Skills",
                keyColumn: "Id",
                keyValue: 16);

            migrationBuilder.DeleteData(
                table: "Skills",
                keyColumn: "Id",
                keyValue: 17);

            migrationBuilder.DeleteData(
                table: "Skills",
                keyColumn: "Id",
                keyValue: 18);

            migrationBuilder.DeleteData(
                table: "Skills",
                keyColumn: "Id",
                keyValue: 19);

            migrationBuilder.DeleteData(
                table: "Skills",
                keyColumn: "Id",
                keyValue: 20);

            migrationBuilder.DeleteData(
                table: "Skills",
                keyColumn: "Id",
                keyValue: 21);

            migrationBuilder.DeleteData(
                table: "Skills",
                keyColumn: "Id",
                keyValue: 22);

            migrationBuilder.DeleteData(
                table: "Skills",
                keyColumn: "Id",
                keyValue: 23);

            migrationBuilder.DeleteData(
                table: "Skills",
                keyColumn: "Id",
                keyValue: 24);

            migrationBuilder.DeleteData(
                table: "Skills",
                keyColumn: "Id",
                keyValue: 25);

            migrationBuilder.DeleteData(
                table: "Skills",
                keyColumn: "Id",
                keyValue: 26);

            migrationBuilder.DeleteData(
                table: "Skills",
                keyColumn: "Id",
                keyValue: 27);

            migrationBuilder.DeleteData(
                table: "Skills",
                keyColumn: "Id",
                keyValue: 28);

            migrationBuilder.DeleteData(
                table: "Skills",
                keyColumn: "Id",
                keyValue: 29);

            migrationBuilder.DeleteData(
                table: "Skills",
                keyColumn: "Id",
                keyValue: 30);

            migrationBuilder.DeleteData(
                table: "Skills",
                keyColumn: "Id",
                keyValue: 31);

            migrationBuilder.DeleteData(
                table: "Skills",
                keyColumn: "Id",
                keyValue: 32);

            migrationBuilder.DeleteData(
                table: "Skills",
                keyColumn: "Id",
                keyValue: 33);

            migrationBuilder.DeleteData(
                table: "Skills",
                keyColumn: "Id",
                keyValue: 34);
        }
    }
}
