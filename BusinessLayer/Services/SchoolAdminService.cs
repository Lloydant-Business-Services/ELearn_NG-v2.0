using BusinessLayer.Infrastructure;
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
    public class SchoolAdminService : ISchoolAdminService
    {
        private readonly ELearnContext _context;
        private readonly IConfiguration _configuration;
        private readonly string baseUrl;
        private readonly string defaultPassword = "1234567";

        public SchoolAdminService(ELearnContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
            baseUrl = _configuration.GetValue<string>("Url:root");


        }

        public async Task<ExcelSheetUploadAggregation> ProcessStudentUpload(IEnumerable<StudentUploadModel> studentList, long departmentId)
        {
            ExcelSheetUploadAggregation uploadAggregation = new ExcelSheetUploadAggregation();
            List<StudentUploadModel> failedUploads = new List<StudentUploadModel>();
            uploadAggregation.SuccessfullUpload = 0;
            uploadAggregation.FailedUpload = 0;
            using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                if(studentList.Count() > 0)
                {
                    foreach (StudentUploadModel student in studentList)
                    {
                        var surname = student.Surname.Trim();
                        var firstname = student.Firstname.Trim();
                        var othername = student.Othername.Trim();
                        var matNo = student.MatricNumber.Trim();
                        StudentUploadModel failedUploadSingle = new StudentUploadModel();

                        
                        
                        var studentPerson = await GetStudentPersonBy(matNo);
                        if(studentPerson == null)
                        {
                            Person person = new Person()
                            {
                                Surname = surname,
                                Firstname = firstname,
                                Othername = othername != null ? othername : null
                            };
                            _context.Add(person);
                            await _context.SaveChangesAsync();

                            var mat_no_slug = Utility.GenerateSlug(matNo);
                            StudentPerson _student_person = new StudentPerson()
                            {
                                MatricNo = matNo,
                                PersonId = person.Id,
                                MatricNoSlug = mat_no_slug,
                                Active = true,
                                DepartmentId = departmentId
                            };
                            _context.Add(_student_person);
                            await _context.SaveChangesAsync();

                            Utility.CreatePasswordHash(defaultPassword, out byte[] passwordHash, out byte[] passwordSalt);
                            User user = new User()
                            {
                                Username = matNo,
                                PersonId = person.Id,
                                RoleId = (int)UserRole.Student,
                                PasswordHash = passwordHash,
                                PasswordSalt = passwordSalt,
                                IsVerified = true,
                                Active = true

                            };
                            _context.Add(user);
                            await _context.SaveChangesAsync();

                            uploadAggregation.SuccessfullUpload += 1;
                        }
                        //Already exists
                        else
                        {
                            failedUploadSingle.Surname = surname;
                            failedUploadSingle.Firstname = firstname;
                            failedUploadSingle.Othername = othername;
                            failedUploadSingle.MatricNumber = matNo;
                            failedUploads.Add(failedUploadSingle);
                            uploadAggregation.FailedUpload += 1;
                        }
                    }
                    await transaction.CommitAsync();
                    uploadAggregation.FailedStudentUploads = failedUploads;
                    
                }
                return uploadAggregation;
            }
            catch(Exception ex)
            {
                transaction.Rollback();
                throw ex;
            }
        }

        public async Task<StudentPerson> GetStudentPersonBy(string MatricNo)
        {
            try
            {
                var matNoSlug = Utility.GenerateSlug(MatricNo);
                return await _context.STUDENT_PERSON.Where(x => x.MatricNoSlug == matNoSlug).FirstOrDefaultAsync();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public async Task<IEnumerable<GetInstitutionUsersDto>> GetAllStudents()
        {
            return await _context.STUDENT_PERSON.Where(a => a.Id > 0)
                .Include(p => p.Person)
                .Select(f => new GetInstitutionUsersDto
                {
                    FullName = f.Person.Surname + " " + f.Person.Firstname + " " + f.Person.Othername,
                    MatricNumber = f.MatricNo,
                    PersonId = f.PersonId,
                    StudentPersonId = f.Id
                    
                })
                .ToListAsync();
        }
        public async Task<IEnumerable<GetInstitutionUsersDto>> GetStudentsDepartmentId(long DepartmentId)
        {
            return await _context.STUDENT_PERSON.Where(a => a.DepartmentId == DepartmentId)
                .Include(p => p.Person)
                .Select(f => new GetInstitutionUsersDto
                {
                    FullName = f.Person.Surname + " " + f.Person.Firstname + " " + f.Person.Othername,
                    MatricNumber = f.MatricNo,
                    PersonId = f.PersonId,
                    StudentPersonId = f.Id

                })
                .ToListAsync();
        }

        public async Task<DetailCountDto> InstitutionDetailCount()
        {
            DetailCountDto countDto = new DetailCountDto();
            var studentCount = await _context.STUDENT_PERSON.Where(d => d.Id > 0).CountAsync();
            var instructorCount = await _context.COURSE_ALLOCATION.Where(d => d.Id > 0).CountAsync();
            var departmentCount = await _context.DEPARTMENT.Where(d => d.Id > 0).CountAsync();
            countDto.AllDepartments = departmentCount;
            countDto.AllStudents = studentCount;
            countDto.AllInstructors = instructorCount;
            return countDto;
        }

        //public async Task<CourseMaterialDto> GetCourseMaterialsBy(long departmentId, long courseId, long instructorId)
        //{
        //    SchoolAdminStatsDto summary = new SchoolAdminStatsDto();
        //    var courseMaterials = await _context.COURSE_CONTENT.Where(x => x.CourseTopic.CourseAllocation.Instructor)
        //}
    }
}
