﻿using System;
using System.Collections.Generic;
using System.Text;

namespace DataLayer.Dtos
{
    public class AssignmentDto
    {
        public long AssignmentId { get; set; }
        public string AssignmentInstruction { get; set; }
        public string AssignmentInText { get; set; }
        public string AssignmentVideoLink { get; set; }
        public string AssignmentUploadLink { get; set; }
        public DateTime DueDate { get; set; }
        public DateTime SetDate { get; set; }
        public decimal MaxScore { get; set; }
        public bool Active { get; set; }
        public bool IsPublished { get; set; }
        public bool IsDeleted { get; set; }
        public string CourseCode { get; set; }
        public string CourseTitle { get; set; }
        public string AssignmentName { get; set; }
        public string InstructorName { get; set; }
    }
}
