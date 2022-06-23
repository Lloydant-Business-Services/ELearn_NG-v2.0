using System;
using System.Collections.Generic;
using System.Text;

namespace DataLayer.Enums
{
    public enum SystemRole
    {
        Admin = 2,
        Instructor,
        HOD,
        Student
    }

    public enum PaymentCheck
    {
        EnabledAndPaid = 1,
        EnabledAndNotPaid,
        Disabled
    }
}
