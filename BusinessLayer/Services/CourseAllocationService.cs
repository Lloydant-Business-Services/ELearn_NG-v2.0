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
    public class CourseAllocationService : ICourseAllocationService
    {
        private readonly IConfiguration _configuration;
        private readonly ELearnContext _context;

        public CourseAllocationService(IConfiguration configuration, ELearnContext context)
        {
            _configuration = configuration;
            _context = context;
        }

        public async Task<ResponseModel> AllocateCourse(AllocateCourseDto dto)
        {
            try
            {
                ResponseModel response = new ResponseModel();
                Course course = await _context.COURSE.Where(c => c.Id == dto.CourseId).FirstOrDefaultAsync();
                User user = await _context.USER.Where(f => f.Id == dto.UserId).FirstOrDefaultAsync();
                User instructor = await _context.USER.Where(f => f.Id == dto.InstructorId).FirstOrDefaultAsync();
                SessionSemester sessionSemester = await _context.SESSION_SEMESTER.Where(s => s.Id == dto.SessionSemesterId).FirstOrDefaultAsync();
                var doesExist = await _context.COURSE_ALLOCATION.Where(c => c.CourseId == dto.CourseId && c.InstructorId == dto.InstructorId && c.CreatedById == dto.UserId && c.SessionSemesterId == dto.SessionSemesterId).FirstOrDefaultAsync();
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
                        SessionSemesterId = dto.SessionSemesterId,
                        Active = true
                    };
                    _context.Add(courseAllocation);
                    await _context.SaveChangesAsync();
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
            return await _context.COURSE_ALLOCATION.Where(f => f.SessionSemester.Active)
                .Include(f => f.Instructor)
                .ThenInclude(p => p.Person)
                .Include(c => c.Course)
                .Select(f => new GetInstructorDto
                { 
                    FullName = f.Instructor.Person.Surname + " " + f.Instructor.Person.Firstname + " " + f.Instructor.Person.Othername,
                    PersonId = f.Instructor.PersonId,
                    CourseCode = f.Course.CourseCode,
                    CourseTitle = f.Course.CourseTitle,
                    Email = f.Instructor.Person.Email,
                    CourseId = f.CourseId

                })
                .ToListAsync();
        }
    }
}
