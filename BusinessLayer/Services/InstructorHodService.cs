using BusinessLayer.Interface;
using DataLayer.Dtos;
using DataLayer.Model;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.Services
{
    public class InstructorHodService : IInstructorHodService
    {
        private readonly IConfiguration _configuration;
        private readonly ELearnContext _context;
        private readonly string defualtPassword = "1234567";

        public InstructorHodService(IConfiguration configuration, ELearnContext context)
        {
            _configuration = configuration;
            _context = context;
        }
        public async Task<ResponseModel> AddCourseInstructorAndHod(AddUserDto userDto)
        {
            ResponseModel response = new ResponseModel();
            using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                User user = new User();
                if (userDto.DepartmentId <= 0)
                {
                    response.Message = "Department not Specified";
                    response.StatusCode = StatusCodes.Status400BadRequest;
                    return response;
                }

                Person person = new Person()
                {
                    Surname = userDto.Surname,
                    Firstname = userDto.Firstname,
                    Othername = userDto.Othername,
                    Email = userDto.Email,
                };
                _context.Add(person);
                await _context.SaveChangesAsync();

                Utility.CreatePasswordHash(defualtPassword, out byte[] passwordHash, out byte[] passwordSalt);
                user.Username = userDto.Email;
                user.RoleId = userDto.RoleId;
                user.IsVerified = true;
                user.Active = true;
                user.PasswordHash = passwordHash;
                user.PasswordSalt = passwordSalt;
                user.PersonId = person.Id;
                _context.Add(user);
                await _context.SaveChangesAsync();
                
                if(userDto.RoleId == (int)Roles.DepartmentAdministrator)
                {
                    var doesExist = await _context.DEPARTMENT_HEADS.Where(d => d.DepartmentId == userDto.DepartmentId).FirstOrDefaultAsync();
                    if (doesExist != null)
                    {
                        response.Message = "HOD already exists for the selected department";
                        response.StatusCode = StatusCodes.Status400BadRequest;
                        return response;
                    }
                    DepartmentHeads departmentHead = new DepartmentHeads()
                    {
                        DepartmentId = userDto.DepartmentId,
                        UserId = user.Id
                    };
                    _context.Add(departmentHead);
                    await _context.SaveChangesAsync();
                }

                else if (userDto.RoleId == (int)Roles.Instructor)
                {
                    
                    InstructorDepartment instructorDepartment = new InstructorDepartment()
                    {
                        //CourseAllocation = courseAllocation != null ? courseAllocation : null,
                        DepartmentId = userDto.DepartmentId,
                        UserId = user.Id
                    };
                    _context.Add(instructorDepartment);
                    await _context.SaveChangesAsync();
                }


                await transaction.CommitAsync();

                return response;

            }
            catch (Exception ex)
            {
                transaction.Rollback();
                throw ex;
            }

        }

        public async Task<IEnumerable<GetInstructorDto>> GetInstututionInstructors()
        {
            try
            {
                return await _context.INSTRUCTOR_DEPARTMENT.Where(f => f.Id > 0)
                .Include(p => p.User)
                .ThenInclude(p => p.Person)
                .Include(d => d.Department)
                .ThenInclude(f => f.FacultySchool)
                .Include(c => c.CourseAllocation)
                .ThenInclude(c => c.Course)
                .Select(f => new GetInstructorDto
                {
                    FullName = f.User.Person.Surname + " " + f.User.Person.Firstname + " " + f.User.Person.Othername,
                    UserId = f.UserId,
                    Email = f.User.Person.Email,
                    Department = f.Department,
                    CourseCode = f.CourseAllocation != null ? f.CourseAllocation.Course.CourseCode : null,
                    CourseTitle = f.CourseAllocation !=null ? f.CourseAllocation.Course.CourseTitle : null,
                    CourseId = f.CourseAllocation !=null ? f.CourseAllocation.CourseId : 0

                })
                .ToListAsync();
            }
            catch(Exception ex)
            {
                throw ex;
            }
            
        }

        public async Task<IEnumerable<GetInstructorDto>> GetAllDepartmentHeads()
        {
            var activeSessionSemester = await GetActiveSessionSemester();
            return await _context.DEPARTMENT_HEADS.Where(d => d.Id > 0)
                .Include(p => p.User)
                .ThenInclude(p => p.Person)
                .Include(d => d.Department)
                .ThenInclude(f => f.FacultySchool)
                .Select(f => new GetInstructorDto
                {
                    FullName = f.User.Person.Surname + " " + f.User.Person.Firstname + " " + f.User.Person.Othername,
                    UserId = f.Id,
                    Email = f.User.Person.Email,
                    Department = f.Department,
                })
                .ToListAsync();
        }
        

        public async Task<IEnumerable<GetInstructorDto>> GetInstructorsByDepartmentId(long departmentId)
        {
            var activeSessionSemester = await GetActiveSessionSemester();
            //return await _context.INSTRUCTOR_DEPARTMENT.Where(d => d.DepartmentId == departmentId && d.CourseAllocation.SessionSemester.Active)
            return await _context.INSTRUCTOR_DEPARTMENT.Where(d => d.DepartmentId == departmentId)
                .Include(d => d.User)
                .ThenInclude(p => p.Person)
                .Include(d => d.Department)
                .ThenInclude(f => f.FacultySchool)
                .Include(c => c.CourseAllocation)
                .ThenInclude(c => c.Course)
                .Select(f => new GetInstructorDto
                {
                    FullName = f.User.Person.Surname + " " + f.User.Person.Firstname + " " + f.User.Person.Othername,
                    UserId = f.UserId,
                    Email = f.User.Person.Email,
                    Department = f.Department,
                    CourseCode = f.CourseAllocation != null ? f.CourseAllocation.Course.CourseCode : null,
                    CourseTitle = f.CourseAllocation != null ? f.CourseAllocation.Course.CourseTitle : null,
                    CourseId = f.CourseAllocation != null ? f.CourseAllocation.CourseId : 0
                })
                
                .Distinct()
                .ToListAsync();
        }
        public async Task<GetSessionSemesterDto> GetActiveSessionSemester()
        {
            return await _context.SESSION_SEMESTER.Where(a => a.Active)
                .Include(s => s.Semester)
                .Include(s => s.Session)
                .Select(f => new GetSessionSemesterDto
                {
                    SemesterName = f.Semester.Name,
                    SessionName = f.Session.Name,
                    SemesterId = f.SemesterId,
                    SessionId = f.SessionId,
                    Id = f.Id
                })
                .FirstOrDefaultAsync();
        }
    }
}
