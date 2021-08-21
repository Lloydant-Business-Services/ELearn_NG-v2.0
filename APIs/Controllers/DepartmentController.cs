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

        [HttpPost("[action]")]
        //[Authorize]
        public async Task<ResponseModel> AddDepartment(DepartmentDto model) => await _service.AddDepartment(model);
        //public async Task<long> PostDepartment([FromBody] Department department) => await _service.Insert(department);

        [HttpGet("[action]")]
        //[Authorize]
        public async Task<IEnumerable<DepartmentDto>> GetDepartments()
        {
            return await _context.DEPARTMENT.Where(a => a.Id > 0).Include(f => f.FacultySchool)
                .Select(f => new DepartmentDto
                {
                    Name = f.Name,
                    FacultyId = f.FacultySchoolId,
                    Id = f.Id
                })
                .ToListAsync();
        }
        [HttpGet("[action]")]
        public async Task<IEnumerable<DepartmentDto>> GetDepartmentsByFacultyId(long facultyId) => await _service.GetDepartmentsByFacultyId(facultyId);
        [HttpGet("{id}")]
        public Department GetById(long id) => _service.GetById(id);
        [HttpPut]
        public async Task<ResponseModel> UpdateDepartment(DepartmentDto model) => await _service.UpdateDepartment(model);
        [HttpGet("GetDepartmentHeadsByFacultyId")]
        public async Task<IEnumerable<GetDepartmentHeadDto>> GetDepartmentHeadsByFaculty(long facultyId) => await _service.GetDepartmentHeadsByFaculty(facultyId);
        [HttpGet("[action]")]
        public async Task<GetDepartmentHeadDto> GetDepartmentHeadByDepartmentId(long departmentId) => await _service.GetDepartmentHeadByDepartmentId(departmentId);
        [HttpGet("[action]")]
        public async Task<IEnumerable<GetDepartmentHeadDto>> GetAllDepartmentHeads() => await _service.GetAllDepartmentHeads();
        [HttpPost("[action]")]
        public async Task<ResponseModel> AssignDepartmentHead(AddDepartmentHeadDto dto) => await _service.AssignDepartmentHead(dto);
        //[HttpDelete]
        //public async Task<ResponseModel> DeleteDepartment(long id) => await _service.DeleteDepartment(id);
        //public async Task<long> UpdateById([FromBody]Department department) => await _service.Update(department);
    }
}
