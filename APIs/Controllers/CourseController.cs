using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BusinessLayer.Interface;
using DataLayer.Dtos;
using DataLayer.Model;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace APIs.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CourseController : ControllerBase
    {
        private readonly ICourseService _service;
        public CourseController(ICourseService service)
        {
            _service = service;
        }

        [HttpPost("AddCourses")]
        public async Task<ResponseModel> PostCourse(AddCourseDto courseDto) => await _service.AddCourse(courseDto);
        [HttpPut("UpdateCourse")]
        public async Task<ResponseModel> UpdateCourseDetail(AddCourseDto dto) => await _service.UpdateCourseDetail(dto);
        [HttpGet("GetAllCourses")]
        public async Task<IEnumerable<AddCourseDto>> GetCourses() => await _service.GetCourses();
    }
}
