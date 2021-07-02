using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BusinessLayer.Interface;
using DataLayer.Model;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace APIs.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DepartmentController : ControllerBase
    {
        private readonly IDepartmentService _service;

        public DepartmentController(IDepartmentService service)
        {
            _service = service;
        }

        [HttpPost]
        //[Authorize]
        public async Task<long> PostDepartment([FromBody] Department department) => await _service.Insert(department);

        [HttpGet]
        [Authorize]
        public IEnumerable<Department> GetAll() => _service.GetAll();
        [HttpGet("{id}")]
        public Department GetById(long id) => _service.GetById(id);
        [HttpPut]
        public async Task<long> UpdateById([FromBody]Department department) => await _service.Update(department);
    }
}
