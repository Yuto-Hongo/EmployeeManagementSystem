using Backend.Models;
using Backend.Enums;
using Backend.Services;

namespace Backend.Data;

public static class AppDbContextSeed
{
    // Šù‚Éƒf[ƒ^‚ª‚ ‚éê‡‚ÍƒXƒLƒbƒv(‰Šú‰»ˆ—‚ğ–h~)
    public static void Seed(AppDbContext context)
    {
        if (context.Employees.Any())
            return;

        if (context.Users.Any())
            return;
        // Salt + Hash ƒpƒXƒ[ƒhì¬
        var salt = AuthService.GenerateSalt();
        var hashPass = AuthService.Hash("Password123", salt);

        var employees = new List<Employee>
        {
            new Employee
            {
                Id = 1,
                UserId = 1,
                FullName = "SeedUser1_Admin",
                DateOfBirth = new DateTime(1990, 1, 1),
                Gender = Gender.’j«,
                Address = "“Œ‹“sa’J‹æ",
                JoinDate = DateTime.Now.AddYears(-5),
                VacationRemaining = 10,
                CurrentWorkplace = "–{Ğ"
            },
            new Employee
            {
                Id = 2,
                UserId = 2,
                FullName = "SeedUser2_General",
                DateOfBirth = new DateTime(1995, 12, 1),
                Gender = Gender.—«,
                Address = "“Œ‹“s¢“c’J‹æ",
                JoinDate = new DateTime(2015, 4, 1),
                VacationRemaining = 20,
                CurrentWorkplace = "í’“æ1"
            },
            new Employee
            {
                Id = 3,
                UserId = 3,
                FullName = "aaa_test3",
                DateOfBirth = new DateTime(1991, 2, 1),
                Gender = Gender.’j«,
                Address = "“Œ‹“s™•À‹æ1-1-1",
                JoinDate = new DateTime(2020, 5, 1),
                VacationRemaining = 10,
                CurrentWorkplace = "í’“æ2"
            },
            new Employee
            {
                Id = 4,
                UserId = 4,
                FullName = "bbb_test4",
                DateOfBirth = new DateTime(1992, 3, 1),
                Gender = Gender.—«,
                Address = "“Œ‹“s™•À‹æ2-2-2",
                JoinDate = new DateTime(2020, 6, 1),
                VacationRemaining = 10,
                CurrentWorkplace = "í’“æ2"
            },
            new Employee
            {
                Id = 5,
                UserId = 5,
                FullName = "ccc_test5",
                DateOfBirth = new DateTime(1993, 4, 1),
                Gender = Gender.’j«,
                Address = "“Œ‹“s™•À‹æ3-3-3",
                JoinDate = new DateTime(2020, 8, 1),
                VacationRemaining = 10,
                CurrentWorkplace = "í’“æ4"
            },
            new Employee
            {
                Id = 6,
                UserId = 6,
                FullName = "ddd_test6",
                DateOfBirth = new DateTime(1994, 5, 1),
                Gender = Gender.—«,
                Address = "“Œ‹“s™•À‹æ4-4-4",
                JoinDate = new DateTime(2020, 9, 1),
                VacationRemaining = 10,
                CurrentWorkplace = "í’“æ5"
            },
            new Employee
            {
                Id = 7,
                UserId = 7,
                FullName = "eee_test7",
                DateOfBirth = new DateTime(1995, 6, 1),
                Gender = Gender.’j«,
                Address = "“Œ‹“s™•À‹æ5-5-5",
                JoinDate = new DateTime(2020, 9, 1),
                VacationRemaining = 10,
                CurrentWorkplace = "í’“æ5"
            },
            new Employee
            {
                Id = 8,
                UserId = 8,
                FullName = "fff_test8",
                DateOfBirth = new DateTime(1996, 7, 1),
                Gender = Gender.—«,
                Address = "“Œ‹“s™•À‹æ6-6-6",
                JoinDate = new DateTime(2020, 10, 1),
                VacationRemaining = 10,
                CurrentWorkplace = "í’“æ6"
            },
            new Employee
            {
                Id = 9,
                UserId = 9,
                FullName = "ggg_test9",
                DateOfBirth = new DateTime(1997, 8, 1),
                Gender = Gender.’j«,
                Address = "“Œ‹“s™•À‹æ7-7-7",
                JoinDate = new DateTime(2020, 11, 1),
                VacationRemaining = 10,
                CurrentWorkplace = "í’“æ7"
            },
            new Employee
            {
                Id = 10,
                UserId = 10,
                FullName = "hhh_test10",
                DateOfBirth = new DateTime(1998, 9, 1),
                Gender = Gender.—«,
                Address = "“Œ‹“s™•À‹æ8-8-8",
                JoinDate = new DateTime(2020, 12, 1),
                VacationRemaining = 10,
                CurrentWorkplace = "–{Ğ"
            }
        };

        var users = new List<User>
        {
            new User
            {
                Id =1,
                Email = "signup_admin@examle.com",
                PasswordHash = hashPass,
                Salt = salt,
                Role = UserRole.Admin
            },
            new User
            {
                Id = 2,
                Email = "signup_general@examle.com",
                PasswordHash = hashPass,
                Salt = salt,
                Role = UserRole.General
            },
            new User
            {
                Id =3,
                Email = "signup_general_1@examle.com",
                PasswordHash = hashPass,
                Salt = salt,
                Role = UserRole.General
            },
            new User
            {
                Id =4,
                Email = "signup_general_2@examle.com",
                PasswordHash = hashPass,
                Salt = salt,
                Role = UserRole.General
            },
            new User
            {
                Id =5,
                Email = "signup_general_3@examle.com",
                PasswordHash = hashPass,
                Salt = salt,
                Role = UserRole.General
            },
            new User
            {
                Id =6,
                Email = "signup_general_4@examle.com",
                PasswordHash = hashPass,
                Salt = salt,
                Role = UserRole.General
            },
            new User
            {
                Id =7,
                Email = "signup_general_5@examle.com",
                PasswordHash = hashPass,
                Salt = salt,
                Role = UserRole.General
            },
            new User
            {
                Id =8,
                Email = "signup_general_6@examle.com",
                PasswordHash = hashPass,
                Salt = salt,
                Role = UserRole.General
            },
            new User
            {
                Id =9,
                Email = "signup_general_7@examle.com",
                PasswordHash = hashPass,
                Salt = salt,
                Role = UserRole.General
            },
            new User
            {
                Id =10,
                Email = "signup_admin_1@examle.com",
                PasswordHash = hashPass,
                Salt = salt,
                Role = UserRole.Admin
            }
        };

        // --- ƒXƒLƒ‹Š„‚è“–‚ÄƒTƒ“ƒvƒ‹ ---
        if (!context.EmployeeSkills.Any())
        {
            var employeeSkills = new List<EmployeeSkill>
            {
                new EmployeeSkill { EmployeeId = 1, SkillId = 1 },
                new EmployeeSkill { EmployeeId = 1, SkillId = 7 },
                new EmployeeSkill { EmployeeId = 2, SkillId = 4 },
                new EmployeeSkill { EmployeeId = 2, SkillId = 5 },
                new EmployeeSkill { EmployeeId = 3, SkillId = 2 },
                new EmployeeSkill { EmployeeId = 3, SkillId = 6 },
                new EmployeeSkill { EmployeeId = 4, SkillId = 1 },
                new EmployeeSkill { EmployeeId = 4, SkillId = 3 },
                new EmployeeSkill { EmployeeId = 5, SkillId = 5 },
                new EmployeeSkill { EmployeeId = 6, SkillId = 8 },
                new EmployeeSkill { EmployeeId = 7, SkillId = 1 },
                new EmployeeSkill { EmployeeId = 7, SkillId = 4 },
                new EmployeeSkill { EmployeeId = 8, SkillId = 3 },
                new EmployeeSkill { EmployeeId = 9, SkillId = 6 },
                new EmployeeSkill { EmployeeId = 10, SkillId = 7 },
            };
            context.EmployeeSkills.AddRange(employeeSkills);
        }

        // DB‚É’Ç‰Á‚µ‚Ä•Û‘¶
        context.Users.AddRange(users);
        context.Employees.AddRange(employees);
        context.SaveChanges();
    }
}