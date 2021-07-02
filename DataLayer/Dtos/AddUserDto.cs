using DataLayer.Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace DataLayer.Dtos
{
    public class AddUserDto:BaseModel
    {
        public string Firstname { get; set; }
        public string Othername { get; set; }
        public string Surname { get; set; }
        public string Username { get; set; }
        public string Email { get; set; }
        //public long PersonId { get; set; }
        //public long UserId { get; set; }
        public long RoleId { get; set; }
        public string FullName { get; set; }
    }
}
