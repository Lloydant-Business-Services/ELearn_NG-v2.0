using DataLayer.Dtos;
using DataLayer.Model;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.Interface
{
    public interface ISchoolAdminService
    {
        Task<ExcelSheetUploadAggregation> ProcessStudentUpload(IEnumerable<StudentUploadModel> studentList, long departmentId);
        Task<IEnumerable<GetInstitutionUsersDto>> GetAllStudents();
        Task<DetailCountDto> InstitutionDetailCount();
        Task<IEnumerable<GetInstitutionUsersDto>> GetStudentsDepartmentId(long DepartmentId);
        Task<bool> DeleteStudent(long studentPersonId);
    }
}
