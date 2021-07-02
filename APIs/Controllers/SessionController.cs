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
    public class SessionController : ControllerBase
    {
        private readonly IRepository<Session> _repo;
        public SessionController(IRepository<Session> repo)
        {
            _repo = repo;
        }

        [HttpPost]
        public async Task<long> AddSesion([FromBody] Session session) => await _repo.Insert(session);
        [HttpGet]
        public IEnumerable<Session> GetAll() => _repo.GetAll();

        [HttpGet("{id}")]
        public Session GetById(long id) => _repo.GetById(id);

        [HttpDelete]
        public void Delete(long id) => _repo.Delete(id);
    }
}
