using System;
using System.Collections.Generic;
using System.Text;

namespace DataLayer.Dtos
{
    public class UpdateAssignmentDto
    {
        public long AssignmentId { get; set; }
        public string AssignmentInstruction { get; set; }
        public string AssignmentInText { get; set; }
        public DateTime DueDate { get; set; }
        public DateTime SetDate { get; set; }
        public decimal MaxScore { get; set; }

        public string AssignmentName { get; set; }
    }
}
