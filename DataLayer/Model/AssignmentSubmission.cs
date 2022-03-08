using System;
using System.Collections.Generic;
using System.Text;

namespace DataLayer.Model
{
    public class AssignmentSubmission : BaseModel
    {
        public string AssignmentInTextSubmission { get; set; }
        public string AssignmentSubmissionUploadLink { get; set; }
        public string AssignmentSubmissionHostedLink { get; set; }
        public string InstructorRemark { get; set; }
        public decimal Score { get; set; }
        public bool? IsGraded { get; set; }
        public DateTime DateSubmitted { get; set; }
        public bool Active { get; set; }
        public long AssignmentId { get; set; }
        public Assignment Assignment { get; set; }
        public long CourseRegistrationId { get; set; }
        public CourseRegistration CourseRegistration { get; set; }
    }
}
