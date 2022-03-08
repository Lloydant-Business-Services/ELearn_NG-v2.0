using System;
using System.Collections.Generic;
using System.Text;

namespace DataLayer.Dtos
{
    public class AssignmentDueDateDto
    {
        public long AssignmentId { get; set; }
        public DateTime DueDate { get; set; }
    }
}
