using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BusinessLayer.Interface;
using DataLayer.Dtos;
using DataLayer.Model;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace APIs.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DepartmentController : ControllerBase
    {
        private readonly IDepartmentService _service;
        private readonly ELearnContext _context;

        public DepartmentController(IDepartmentService service, ELearnContext context)
        {
            _service = service;
            _context = context;
        }

        [HttpPost]
        //[Authorize]
        public async Task<ResponseModel> AddDepartment(DepartmentDto model) => await _service.AddDepartment(model);
        //public async Task<long> PostDepartment([FromBody] Department department) => await _service.Insert(department);

        [HttpGet]
        //[Authorize]
        public async Task<IEnumerable<Department>> GetAll()
        {
            return await _context.DEPARTMENT.Where(a => a.Id > 0).Include(f => f.FacultySchool)
                .ToListAsync();
        }
        [HttpGet("{id}")]
        public Department GetById(long id) => _service.GetById(id);
        [HttpPut]
        public async Task<ResponseModel> UpdateDepartment(DepartmentDto model) => await _service.UpdateDepartment(model);
        [HttpDelete]
        public async Task<ResponseModel> DeleteDepartment(long id) => await _service.DeleteDepartment(id);
        //public async Task<long> UpdateById([FromBody]Department department) => await _service.Update(department);
    }
}
