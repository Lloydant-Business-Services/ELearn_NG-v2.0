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
    public class SemsterController : ControllerBase
    {
        private readonly IRepository<Semester> _repo;

        public SemsterController(IRepository<Semester> repo)
        {
            _repo = repo;
        }
        [HttpGet("GetAllSemester")]
        public IEnumerable<Semester> GetSemesters() => _repo.GetAll();

        [HttpPost("AddSemester")]
        public async Task<long> PostSemester([FromBody] Semester semester) => await _repo.Insert(semester);

        [HttpGet("{id}")]
        public Semester GetById(long id) => _repo.GetById(id);
        //[HttpPut("UpdateSemester")]
        //public async Task<long> EditSemster(Semester semester) => await _repo.Update(semester);

        //[HttpDelete]
        //public void Delete(long id) => _repo.Delete(id);


    }

    
}
