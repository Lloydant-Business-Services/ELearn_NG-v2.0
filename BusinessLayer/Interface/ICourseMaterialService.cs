﻿using DataLayer.Dtos;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.Interface
{
   public interface ICourseMaterialService
    {
        Task<long> CreateCourseTopic(AddCourseTopicDto addCourseTopicDto);
        Task<IEnumerable<GetCourseTopicDto>> GetCourseTopicBy(long CourseAllocationId);
        Task<long> AddCourseContent(AddCourseContentDto addCourseContentDto, string filePath, string directory);
        Task<IEnumerable<GetCourseContentDto>> GetContentBy(long TopicId);
        Task<bool> DeleteCourseContent(long courseContentId);
        Task<bool> DeleteCourseTopic(long TopicId);
        Task<IEnumerable<AssignmentListDto>> ListAssignmentByCourseId(long courseId);
    }
}