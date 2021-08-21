using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Text;

namespace DataLayer.Dtos
{
    public class StudentAssignmentSubmissionDto
    {
        public long StudentUserId { get; set; }
        public string AssignmentInText { get; set; }
        public string AssignmentHostedLink { get; set; }
        public IFormFile AssignmentUpload { get; set; }
        public long AssignmentId { get; set; }
    }
}
