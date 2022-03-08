using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Text;

namespace DataLayer.Dtos
{
    public class AddAssignmentDto
    {
        public string Name { get; set; }
        public string AssignmentInstruction { get; set; }
        public string AssignmentInText { get; set; }
        public string AssignmentVideoLink { get; set; }
        public IFormFile AssignmentUpload { get; set; }
        public DateTime DueDate { get; set; }
        public decimal MaxScore { get; set; }
        public long CourseAllocationId { get; set; }
    }
}
