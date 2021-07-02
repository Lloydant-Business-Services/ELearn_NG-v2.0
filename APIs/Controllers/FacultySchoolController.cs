using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BusinessLayer.Interface;
using DataLayer.Model;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace APIs.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FacultySchoolController : ControllerBase
    {
        private readonly IFacultyService _service;
        public FacultySchoolController(IFacultyService service)
        {
            _service = service;
        }

        [HttpPost]
        public async Task<long> AddFacultySchool([FromBody] FacultySchool facultySchool) => await _service.Insert(facultySchool);

        [HttpGet]
        public IEnumerable<FacultySchool> GetAll() => _service.GetAll();
        [HttpGet("{id}")]
        public FacultySchool GetBy(long id) => _service.GetById(id);
        [HttpPut]
        public async Task<long> Update([FromBody] FacultySchool facultySchool) => await _service.Update(facultySchool);
    }
}
