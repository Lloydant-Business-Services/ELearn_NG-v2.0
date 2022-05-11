using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BusinessLayer.Interface;
using DataLayer.Model;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace APIs.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SemsterController : ControllerBase
    {
        private readonly IRepository<Semester> _repo;
        private readonly ELearnContext _context;

        public SemsterController(IRepository<Semester> repo, ELearnContext context)
        {
            _repo = repo;
            _context = context;
        }
        [HttpGet("GetAllSemester")]
        public async Task<IEnumerable<Semester>> GetSemesters()
        {
            return await _context.SEMESTER.Where(x => x.Active)
                .OrderBy(x => x.SortOrder)
                .ToListAsync();
        }

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
