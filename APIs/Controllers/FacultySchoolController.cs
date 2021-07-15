using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BusinessLayer.Interface;
using DataLayer.Dtos;
using DataLayer.Model;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace APIs.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FacultySchoolController : ControllerBase
    {
        private readonly IFacultyService _service;
        private readonly ELearnContext _context;
        public FacultySchoolController(IFacultyService service, ELearnContext context)
        {
            _service = service;
            _context = context;
        }

        [HttpPost("[action]")]
        public async Task<ResponseModel> AddFacultySchool(FacultyDto model) => await _service.AddFacultySchool(model);
        //public async Task<long> AddFacultySchool([FromBody] FacultySchool facultySchool) => await _service.Insert(facultySchool);

        [HttpGet("[action]")]
        public async Task<IEnumerable<FacultyDto>> GetFaculties()
        {
            return await _context.FACULTY_SCHOOL.Where(a => a.Id > 0)
                .Select(f => new FacultyDto
                {
                    Name = f.Name,
                    Id = f.Id
                }).ToListAsync();
                
        }
        [HttpGet("{id}")]
        public FacultySchool GetBy(long id) => _service.GetById(id);
        [HttpPut]
        public async Task<ResponseModel> UpdateFacultySchool(FacultyDto model) => await _service.UpdateFacultySchool(model);
        //[HttpDelete]
        //public async Task<ResponseModel> DeleteFacultySchool(long id) => await _service.DeleteFacultySchool(id);
    }
}
