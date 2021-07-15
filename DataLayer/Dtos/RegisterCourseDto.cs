using System;
using System.Collections.Generic;
using System.Text;

namespace DataLayer.Dtos
{
    public class RegisterCourseDto
    {
        public long SessionSemesterId { get; set; }
        public long CourseAllocationId { get; set; }
        public long StudentPersonId { get; set; }
    }
}
