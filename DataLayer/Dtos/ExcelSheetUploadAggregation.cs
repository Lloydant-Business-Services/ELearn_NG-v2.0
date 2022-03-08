using System;
using System.Collections.Generic;
using System.Text;

namespace DataLayer.Dtos
{
    public class ExcelSheetUploadAggregation
    {
        public long SuccessfullUpload { get; set; }
        public long FailedUpload { get; set; }
        public List<StudentUploadModel> FailedStudentUploads { get; set; }
    }
}
