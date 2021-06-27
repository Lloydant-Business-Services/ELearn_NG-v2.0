using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace DataLayer.Model
{
    public class Person:BaseModel
    {
        [MaxLength(100)]
        public string FirstName { get; set; }
        [MaxLength(100)]
        public string LastName { get; set; }
        [MaxLength(100)]
        public string SurName { get; set; }
        [MaxLength(50)]
        public string PhoneNo { get; set; }
        [MaxLength(50)]
        public string Email { get; set; }
        public bool Active { get; set; }
        public long GenderId { get; set; }
        public virtual Gender Gender { get; set; }
    }
}
