using DataLayer.Dtos;
using DataLayer.Model;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.Interface
{
    public interface IInstructorHodService 
    {
        Task<IEnumerable<GetInstructorDto>> GetInstututionInstructors();
        Task<ResponseModel> AddCourseInstructorAndHod(AddUserDto userDto);
        Task<IEnumerable<GetInstructorDto>> GetInstructorsByDepartmentId(long departmentId);
        Task<IEnumerable<GetInstructorDto>> GetAllDepartmentHeads();
        Task<IEnumerable<InstructorCoursesDto>> GetInstructorCoursesByUserId(long instructorUserId);
        Task<IEnumerable<GetInstructorDto>> GetInstructorsByFacultyId(long facultyId);
    }
}
