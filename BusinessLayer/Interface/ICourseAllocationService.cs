﻿using DataLayer.Dtos;
using DataLayer.Model;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.Interface
{
    public interface ICourseAllocationService
    {
        Task<ResponseModel> AllocateCourse(AllocateCourseDto dto);
        Task<IEnumerable<GetInstructorDto>> GetInstututionInstructors();
    }
}
