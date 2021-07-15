using BusinessLayer.Interface;
using DataLayer.Dtos;
using DataLayer.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.Services
{
    public class CourseRegistrationService : ICourseRegistrationService
    {
        private readonly IConfiguration _configuration;
        private readonly ELearnContext _context;

        public CourseRegistrationService(IConfiguration configuration, ELearnContext context)
        {
            _configuration = configuration;
            _context = context;
        }

        public async Task<ResponseModel> RegisterCourses(RegisterCourseDto dto)
        {
            try
            {
                ResponseModel response = new ResponseModel();
                CourseRegistration courseRegistration = new CourseRegistration()
                {
                    StudentPersonId = dto.StudentPersonId,
                    SessionSemesterId = dto.SessionSemesterId,
                    CourseAllocationId = dto.CourseAllocationId,
                    DateRegistered = DateTime.Now,
                    Active = true
                };
                _context.Add(courseRegistration);
                await _context.SaveChangesAsync();
                return response;
            }
            catch(Exception ex)
            {
                throw ex;
            }
        }
        public async Task<IEnumerable<GetRegisteredCoursesDto>> GetRegisteredCourses(long personId, long sessionSemesterId)
        {
            var getPerson = await _context.STUDENT_PERSON.Where(s => s.PersonId == personId).FirstOrDefaultAsync();
            var courseRegistartion = await _context.COURSE_REGISTRATION.Where(f => f.StudentPersonId == getPerson.Id && f.SessionSemesterId == sessionSemesterId)
                .Include(c => c.CourseAllocation)
                .ThenInclude(c => c.Course)
                .Include(c => c.CourseAllocation)
                .ThenInclude(p => p.Instructor)
                .ThenInclude(f => f.Person)
                .Select(f => new GetRegisteredCoursesDto {
                    CourseTitle = f.CourseAllocation.Course.CourseTitle,
                    CourseCode = f.CourseAllocation.Course.CourseCode,
                    CourseLecturer = f.CourseAllocation.Instructor.Person.Surname + " " + f.CourseAllocation.Instructor.Person.Firstname + " " + f.CourseAllocation.Instructor.Person.Othername,
                    CourseId = f.CourseAllocation.CourseId
                })
                .ToListAsync();
            return courseRegistartion;
        }
    }
}
