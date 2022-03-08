using System;
using System.Collections.Generic;
using System.Text;

namespace DataLayer.Dtos
{
    public class ComprehensiveAssignmentCumulativeDto
    {
        public decimal CumulativeScore { get; set; }
        public string StudentName { get; set; }
        public string MatricNumber { get; set; }
        public List<DetailDto> DetailList { get; set; }
    }
    public class DetailDto
    {
        public string AssignmentName { get; set; }
        public decimal Score { get; set; }
    }
}
