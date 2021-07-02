using BusinessLayer.Interface;
using DataLayer.Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace BusinessLayer.Services
{
    public class FacultySchoolService : Repository<FacultySchool>, IFacultyService
    {
        public FacultySchoolService(ELearnContext context) : base(context) { }
    }
}
