using BusinessLayer.Interface;
using DataLayer.Dtos;
using DataLayer.Model;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.Services
{
    public class DepartmentService : Repository<Department>, IDepartmentService
    {

        private readonly IConfiguration _configuration;
        public readonly string baseUrl;
        ResponseModel response = new ResponseModel();

        public DepartmentService(ELearnContext context, IConfiguration configuration)
            : base(context)
        {
            _configuration = configuration;
            baseUrl = configuration.GetValue<string>("Url:root");

        }


        public async Task<ResponseModel> AddDepartment(DepartmentDto model)
        {
            try
            {
                Department dept = new Department();
                var d_slug = Utility.GenerateSlug(model.Name);
                var doesExist = await _context.DEPARTMENT.Where(f => f.slug == d_slug).FirstOrDefaultAsync();
                if (doesExist != null)
                {
                    response.StatusCode = StatusCodes.Status208AlreadyReported;
                    response.Message = "Faculty/School Already Added";
                    return response;
                }

                dept.Name = model.Name;
                dept.slug = d_slug;
                dept.FacultySchoolId = model.FacultyId;
                dept.Active = true;
                dept.DateCreated = DateTime.Now;
                _context.Add(dept);
                await _context.SaveChangesAsync();
                return response;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<ResponseModel> UpdateDepartment(DepartmentDto model)
        {
            try
            {
                Department dept = await _context.DEPARTMENT.Where(f => f.Id == model.Id).FirstOrDefaultAsync();
                if (dept == null)
                    throw new NullReferenceException("Department not found");
                var d_slug = Utility.GenerateSlug(model.Name);
                dept.Name = model.Name;
                dept.slug = d_slug;
                dept.Active = true;
                _context.Update(dept);
                await _context.SaveChangesAsync();
                return response;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public async Task<ResponseModel> DeleteDepartment(long id)
        {
            try
            {
                Department dept = await _context.DEPARTMENT.Where(f => f.Id == id).FirstOrDefaultAsync();
                if (dept != null)
                {
                    _context.Remove(dept);
                    await _context.SaveChangesAsync();
                    response.StatusCode = StatusCodes.Status200OK;
                    response.Message = "deleted";
                }
                return response;

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
