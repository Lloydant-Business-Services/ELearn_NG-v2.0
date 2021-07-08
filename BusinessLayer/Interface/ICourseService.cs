using DataLayer.Dtos;
using DataLayer.Model;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.Interface
{
    public interface ICourseService
    {
        Task<ResponseModel> AddCourse(AddCourseDto courseDto);
        Task<ResponseModel> UpdateCourseDetail(AddCourseDto dto);
        Task<IEnumerable<AddCourseDto>> GetCourses();
        //Task<ResponseModel> AllocateCourse(AllocateCourseDto dto);
    }
}
