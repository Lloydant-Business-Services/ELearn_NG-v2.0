using DataLayer.Dtos;
using DataLayer.Model;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.Interface
{
    public interface IDepartmentService:IRepository<Department>
    {
        Task<ResponseModel> AddDepartment(DepartmentDto model);
        Task<ResponseModel> UpdateDepartment(DepartmentDto model);
        Task<ResponseModel> DeleteDepartment(long id);
    }
}
