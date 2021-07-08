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
    public class CourseAllocationController : ControllerBase
    {
        private readonly ICourseAllocationService _service;
        public CourseAllocationController(ICourseAllocationService service)
        {
            _service = service;
        }

        [HttpPost("AllocateCourse")]
        public async Task<ResponseModel> AllocateCourse(AllocateCourseDto dto) => await _service.AllocateCourse(dto);
        [HttpGet("GetAllInstructors")]
        public async Task<IEnumerable<GetInstructorDto>> GetInstututionInstructors() => await _service.GetInstututionInstructors();
    }
}
