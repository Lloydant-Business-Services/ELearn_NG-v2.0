using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace DataLayer.Model
{
    public class StudentPayment : BaseModel
    {
        [Required]
        public long? LevelId { get; set; }
        public Level Level { get; set; }
        public long? SessionId { get; set; }
        public Session Session { get; set; }
        public decimal? Amount { get; set; }
        public bool? Active { get; set; }
        [StringLength(500)]
        public string Description { get; set; }
        [StringLength(20)]
        public string StatusCode { get; set; }
        public DateTime Created { get; set; }
        [StringLength(150)]
        public string ClientPortalIdentifier { get; set; }// eg invoice number
        public long? DepartmentId { get; set; }
        public Department Department { get; set; }
        public long? ProgrammeId { get; set; }
        [StringLength(200)]
        public string GatewayCode { get; set; }
        [StringLength(20)]
        public string SystemCode { get; set; }
        [StringLength(100)]
        [Required]
        public string SystemPaymentReference { get; set; }
        public string PaymentGateway { get; set; }
        [Required]
        public long StudentPersonId { get; set; }
        public StudentPerson StudentPerson { get; set; }
        public int? PaymentMode { get; set; }//eg card, bankteller,transfer
        public DateTime? DatePaid { get; set; }
        public bool? IsPaid { get; set; }

    }

}
