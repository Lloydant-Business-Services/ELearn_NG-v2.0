using BusinessLayer.Interface;
using DataLayer.Dtos;
using DataLayer.Model;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace APIs.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class InstructorHodController : ControllerBase
    {
        private readonly IInstructorHodService _service;
        public InstructorHodController(IInstructorHodService service)
        {
            _service = service;
        }

        [HttpGet("[action]")]
        public async Task<IEnumerable<GetInstructorDto>> GetInstututionInstructors() => await _service.GetInstututionInstructors();
        [HttpPost("[action]")]
        public async Task<ResponseModel> AddCourseInstructorAndHod(AddUserDto userDto) => await _service.AddCourseInstructorAndHod(userDto);
        [HttpGet("[action]")]
        public async Task<IEnumerable<GetInstructorDto>> GetInstructorsByDepartmentId(long departmentId) => await _service.GetInstructorsByDepartmentId(departmentId);
        [HttpGet("[action]")]
        public async Task<IEnumerable<GetInstructorDto>> GetAllDepartmentHeads() => await _service.GetAllDepartmentHeads();
        [HttpGet("[action]")]
        public async Task<IEnumerable<InstructorCoursesDto>> GetInstructorCoursesByUserId(long instructorUserId) => await _service.GetInstructorCoursesByUserId(instructorUserId);
        [HttpGet("[action]")]
        public async Task<IEnumerable<GetInstructorDto>> GetInstructorsByFacultyId(long facultyId) => await _service.GetInstructorsByFacultyId(facultyId);
    }
}
