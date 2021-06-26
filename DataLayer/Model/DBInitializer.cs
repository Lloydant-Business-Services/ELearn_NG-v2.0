using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;

namespace DataLayer.Model
{
    public class DBInitializer
    {
        public async static void Initialize(IServiceProvider serviceProvider)
        {
            using var context = new ELearnContext(serviceProvider.GetRequiredService<DbContextOptions<ELearnContext>>());
            context.Database.EnsureCreated();
            // Look for any students.
            if (await context.ROLE.AnyAsync())
            {
                return;   // DB has been seeded
            }

            var roles = new Role[]
              {
                    new Role{ Active = true, Name = "Super Admin"},
                    new Role{ Active = true, Name = "ICT DIRECTOR"},
                    new Role{ Active = true, Name = "INSTRUCTOR"},
                    new Role{ Active = true, Name = "STUDENT"},
              };
            foreach (Role role in roles)
            {
                context.Add(role);
            }
            await context.SaveChangesAsync();

            var genders = new Gender[]
            {
                new Gender{ Active = true, Name = "Male"},
                new Gender{ Active = true, Name = "Female"},
            };
            foreach (Gender gender in genders)
            {
                context.Add(gender);
            }
            await context.SaveChangesAsync();
           

            var securityQuestions = new SecurityQuestion[]
           {
                new SecurityQuestion{ Active = true, Name = "Mother's Maiden Name"},
                new SecurityQuestion{ Active = true, Name = "Favorite Food"},
                new SecurityQuestion{ Active = true, Name = "First Car"},
                new SecurityQuestion{ Active = true, Name = "Last Vacation"},
           };
            foreach (SecurityQuestion securityQuestion in securityQuestions)
            {
                context.Add(securityQuestion);
            }
            await context.SaveChangesAsync();

           
            //var personTypes = new PersonType[]
            //   {
            //    new PersonType{ Active = true, Name = "Staff"},
            //    new PersonType{ Active = true, Name = "Student"},
            //    new PersonType{ Active = true, Name = "Admin"},
            //   };
            //foreach (PersonType personType in personTypes)
            //{
            //    context.Add(personType);
            //}
            //await context.SaveChangesAsync();

            //var sessions = new Session[]
            //   {
            //    new Session{ Active = true, Name = "2018/2019"},
            //    new Session{ Active = true, Name = "2019/2020"},
            //    new Session{ Active = true, Name = "2020/2021"},
            //   };
            //foreach (Session session in sessions)
            //{
            //    context.Add(session);
            //}
            //await context.SaveChangesAsync();
            //var semesters = new Semester[]
            //   {
            //    new Semester{ Active = true, Name = "First Semester"},
            //    new Semester{ Active = true, Name = "Second Semester"},
            //    new Semester{ Active = true, Name = "Third Semester"},
            //   };
            //foreach (Semester semester in semesters)
            //{
            //    context.Add(semester);
            //}
            //await context.SaveChangesAsync();
            //var answerOptions = new AnswerOptions[]
            //   {
            //    new AnswerOptions{ Active = true, Name = "a"},
            //    new AnswerOptions{ Active = true, Name = "b"},
            //    new AnswerOptions{ Active = true, Name = "c"},
            //    new AnswerOptions{ Active = true, Name = "d"},
            //    new AnswerOptions{ Active = true, Name = "e"},
            //    new AnswerOptions{ Active = true, Name = "f"},
            //   };
            //foreach (AnswerOptions answerOption in answerOptions)
            //{
            //    context.Add(answerOption);
            //}
            //await context.SaveChangesAsync();

        }

    }
}
