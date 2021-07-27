﻿using BusinessLayer.Interface;
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
    public class CourseAllocationService : ICourseAllocationService
    {
        private readonly IConfiguration _configuration;
        private readonly ELearnContext _context;
        private readonly string defualtPassword = "1234567";

        public CourseAllocationService(IConfiguration configuration, ELearnContext context)
        {
            _configuration = configuration;
            _context = context;
        }

        public async Task<ResponseModel> AllocateCourse(AllocateCourseDto dto)
        {
            using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                ResponseModel response = new ResponseModel();
                Course course = await _context.COURSE.Where(c => c.Id == dto.CourseId).FirstOrDefaultAsync();
                User user = await _context.USER.Where(f => f.Id == dto.UserId).FirstOrDefaultAsync();
                User instructor = await _context.USER.Where(f => f.Id == dto.InstructorId).FirstOrDefaultAsync();
                var activeSesionSemester = await GetActiveSessionSemester();
                SessionSemester sessionSemester = await _context.SESSION_SEMESTER.Where(s => s.Id == activeSesionSemester.Id).FirstOrDefaultAsync();
                var doesExist = await _context.COURSE_ALLOCATION.Where(c => c.CourseId == dto.CourseId && c.SessionSemesterId == activeSesionSemester.Id).FirstOrDefaultAsync();
                InstructorDepartment instructorDepartment = await _context.INSTRUCTOR_DEPARTMENT.Where(x => x.UserId == instructor.Id).FirstOrDefaultAsync();

                if (doesExist != null)
                {
                    response.StatusCode = StatusCodes.Status208AlreadyReported;
                    response.Message = "Course Already assigned for the specified session semester";
                }
                if (course != null && user != null && instructor != null && sessionSemester != null)
                {
                    
                    CourseAllocation courseAllocation = new CourseAllocation()
                    {
                        CourseId = course.Id,
                        InstructorId = instructor.Id,
                        CreatedById = user.Id,
                        DateCreated = DateTime.Now,
                        SessionSemesterId = activeSesionSemester.Id,
                        Active = true
                    };
                    _context.Add(courseAllocation);
                    await _context.SaveChangesAsync();

                    instructorDepartment.CourseAllocationId = courseAllocation.Id;
                    _context.Update(instructorDepartment);
                    await _context.SaveChangesAsync();

                    await transaction.CommitAsync();
                    return response;

                }
                return null;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<IEnumerable<GetInstructorDto>> GetInstututionInstructors()
        {
            return await _context.USER.Where(f => f.RoleId == 3)
                .Include(p => p.Person)
                .Select(f => new GetInstructorDto
                { 
                    FullName = f.Person.Surname + " " + f.Person.Firstname + " " + f.Person.Othername,
                    UserId = f.Id
                
                })
                .ToListAsync();
        }
        public async Task<IEnumerable<GetCourseInstructorDto>> GetInstructorsByDepartmentId(long departmentId)
        {
            var activeSessionSemester = await GetActiveSessionSemester();
            return await _context.INSTRUCTOR_DEPARTMENT.Where(d => d.DepartmentId == departmentId && d.CourseAllocation.SessionSemester.Active)
                .Include(d => d.User)
                .ThenInclude(p => p.Person)
                .Include(c => c.CourseAllocation)
                .ThenInclude(s => s.SessionSemester)
                .Select(f => new GetCourseInstructorDto
                {
                    Fullname = f.User.Person.Surname + " " + f.User.Person.Firstname + " " + f.User.Person.Othername,
                    UserId = f.UserId
                })
                .ToListAsync();
        }
        public async Task<long> AddCourseInstructorAndHod(AddUserDto userDto)
        {
            using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                User user = new User();
                if (userDto.DepartmentId <= 0)
                    throw new NullReferenceException("Department not Specified");

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

                InstructorDepartment instructorDepartment = new InstructorDepartment()
                {
                    //CourseAllocationId = userDto.CourseAllocationId,
                    DepartmentId = userDto.DepartmentId,
                    UserId = user.Id
                };
                _context.Add(instructorDepartment);
                await _context.SaveChangesAsync();

                await transaction.CommitAsync();

                return StatusCodes.Status200OK;

            }
            catch (Exception ex)
            {
                transaction.Rollback();
                throw ex;
            }

        }
        public async Task<IEnumerable<GetCourseInstructorDto>> GetAllDepartmentHeads()
        {
            var activeSessionSemester = await GetActiveSessionSemester();
            return await _context.USER.Where(d => d.Id > 0 && d.RoleId == 4)
                .Include(p => p.Person)
                //.Include(c => c.CourseAllocation)
                //.ThenInclude(s => s.SessionSemester)
                .Select(f => new GetCourseInstructorDto
                {
                    Fullname = f.Person.Surname + " " + f.Person.Firstname + " " + f.Person.Othername,
                    UserId = f.Id
                })
                .ToListAsync();
        }

        public async Task<IEnumerable<GetCourseInstructorDto>> GetAll(long departmentId)
        {
            var activeSessionSemester = await GetActiveSessionSemester();
            return await _context.INSTRUCTOR_DEPARTMENT.Where(d => d.DepartmentId == departmentId && d.CourseAllocation.SessionSemester.Active)
                .Include(d => d.User)
                .ThenInclude(p => p.Person)
                .Include(c => c.CourseAllocation)
                .ThenInclude(s => s.SessionSemester)
                .Select(f => new GetCourseInstructorDto
                {
                    Fullname = f.User.Person.Surname + " " + f.User.Person.Firstname + " " + f.User.Person.Othername,
                    UserId = f.UserId
                })
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