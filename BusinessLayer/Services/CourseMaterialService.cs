﻿using BusinessLayer.Interface;
using DataLayer.Dtos;
using DataLayer.Model;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.Services
{
    public class CourseMaterialService : ICourseMaterialService
    {
        private readonly IConfiguration _configuration;
        private readonly ELearnContext _context;
        private readonly string baseUrl;

        public CourseMaterialService(IConfiguration configuration, ELearnContext context)
        {
            _configuration = configuration;
            _context = context;
            baseUrl = _configuration.GetValue<string>("Url:root");

        }


        public async Task<long> CreateCourseTopic(AddCourseTopicDto addCourseTopicDto)
        {
            if (addCourseTopicDto?.CourseAllocationId > 0 && addCourseTopicDto?.TopicName != null && addCourseTopicDto?.StartDate != null)
            {
                var course = await _context.COURSE_ALLOCATION.Where(f => f.Id == addCourseTopicDto.CourseAllocationId && f.Active).FirstOrDefaultAsync();
                if (course == null)
                    throw new NullReferenceException("Selected Course does not exist");
                CourseTopic courseTopic = new CourseTopic()
                {
                    Description = addCourseTopicDto.TopicDescription,
                    StartDate = addCourseTopicDto.StartDate,
                    EndDate = addCourseTopicDto.EndDate,
                    SetDate = DateTime.UtcNow,
                    CourseAllocationId = course.Id,
                    Active = true,
                    IsArchieved = false,
                    Topic = addCourseTopicDto.TopicName,

                };
                _context.Add(courseTopic);
                var created = await _context.SaveChangesAsync();
                if (created > 0)
                {
                    return StatusCodes.Status200OK;
                }
            }
            else
            {
                throw new NullReferenceException("Please, provide all the required fields");
            }
            return 0;
        }

        public async Task<IEnumerable<GetCourseTopicDto>> GetCourseTopicBy(long CourseAllocationId)
        {
            if (CourseAllocationId == 0)
                throw new NullReferenceException("Please provide query parameter");
            var courseTopics = await _context.COURSE_TOPIC.Where(f => f.CourseAllocationId == CourseAllocationId && !f.IsArchieved)
                .Include(f => f.CourseAllocation)
                .ThenInclude(c => c.Course)
                .ThenInclude(f => f.User)
                .ThenInclude(f => f.Person)
                .Select(f => new GetCourseTopicDto
                {
                    //CourseAccessCode = f.CourseAllocation.CourseAccessCode,
                    SetDate = f.SetDate,
                    StartDate = f.StartDate,
                    TopicDescription = f.Description,
                    CourseCode = f.CourseAllocation.Course.CourseCode,
                    CourseId = f.CourseAllocation.Course.Id,
                    CourseTitle = f.CourseAllocation.Course.CourseTitle,
                    TopicId = f.Id,
                    TopicName = f.Topic,
                    InstructorEmail = f.CourseAllocation.Instructor.Username,
                    InstructorName = f.CourseAllocation.Instructor.Person.Surname + " " + f.CourseAllocation.Instructor.Person.Firstname + " " + f.CourseAllocation.Instructor.Person.Othername
                })
                .ToListAsync();
            return courseTopics;
        }

        public async Task<long> AddCourseContent(AddCourseContentDto addCourseContentDto, string filePath, string directory)
        {
            if (addCourseContentDto?.CourseTopicId > 0)
            {
                var courseTopic = await _context.COURSE_TOPIC
                    .Include(f => f.CourseAllocation)
                    .ThenInclude(c => c.Course)
                    .Where(f => f.Id == addCourseContentDto.CourseTopicId).FirstOrDefaultAsync();
                if (courseTopic == null)
                    throw new NullReferenceException("Course Topic does not exist");
                var saveNoteLink = string.Empty;
                if (addCourseContentDto.Note != null)
                {
                    string fileNamePrefix = courseTopic.Topic + "_" + courseTopic.CourseAllocationId + "_" + DateTime.Now.Millisecond;
                    saveNoteLink = await GetNoteUploadLink(addCourseContentDto.Note, filePath, directory, fileNamePrefix);
                }

                CourseContent courseContent = new CourseContent()
                {
                    Active = true,
                    CourseTopic = courseTopic,
                    Link = addCourseContentDto.VideoLink,
                    SetDate = DateTime.UtcNow,
                    LiveStream = addCourseContentDto.StreamLink,
                    Material = saveNoteLink,
                    ContentTitle = addCourseContentDto.ContentTitle
                };
                _context.Add(courseContent);
                var created = await _context.SaveChangesAsync();
                if (created > 0)
                    return StatusCodes.Status200OK;
            }
            else
            {
                throw new NullReferenceException("Please, select topic to continue");
            }
            return 0;
        }

        public async Task<IEnumerable<GetCourseContentDto>> GetContentBy(long TopicId)
        {
            if (TopicId == 0)
                throw new NullReferenceException("Please provide query parameter");
            var courseContentDtos = await _context.COURSE_CONTENT
                .Include(f => f.CourseTopic)
                .Where(f => f.CourseTopicId == TopicId && f.CourseTopic.IsArchieved == false && !f.IsArchieved)
                .Select(f => new GetCourseContentDto
                {
                    LiveStreamLink = f.LiveStream,
                    NoteLink = baseUrl + f.Material,
                    TopicDescription = f.CourseTopic.Description,
                    StartTime = f.CourseTopic.StartDate,
                    TopicName = f.CourseTopic.Topic,
                    VideoLink = f.Link,
                    ContentTitle = f.ContentTitle

                })
                .ToListAsync();
            return courseContentDtos;
        }

        public async Task<bool> DeleteCourseContent(long courseContentId)
        {
            if (courseContentId == 0)
                throw new NullReferenceException("Please provide query parameter");
            var courseContent = await _context.COURSE_CONTENT.Where(f => f.Id == courseContentId).FirstOrDefaultAsync();
            if (courseContent?.Id > 0)
            {
                courseContent.IsArchieved = true;
                _context.Update(courseContent);
                await _context.SaveChangesAsync();
                return true;
            }
            return false;
        }


        public async Task<bool> DeleteCourseTopic(long TopicId)
        {
            if (TopicId == 0)
                throw new NullReferenceException("Please provide query parameter");
            var courseTopic = await _context.COURSE_CONTENT.Where(f => f.Id == TopicId).FirstOrDefaultAsync();
            if (courseTopic?.Id > 0)
            {
                courseTopic.IsArchieved = true;
                _context.Update(courseTopic);
                await _context.SaveChangesAsync();
                return true;
            }
            return false;
        }
        public async Task<string> GetNoteUploadLink(IFormFile file, string filePath, string directory, string givenFileName)
        {

            var noteUrl = string.Empty;

            var validFileSize = (1024 * 1024);//1mb
            List<string> validFileExtension = new List<string>();
            validFileExtension.Add(".pdf");
            validFileExtension.Add(".doc");
            validFileExtension.Add(".docx");
            validFileExtension.Add(".xlx");
            validFileExtension.Add(".xlxs");
            validFileExtension.Add(".docx");
            validFileExtension.Add(".ppt");
            if (file.Length > 0)
            {

                var extType = Path.GetExtension(file.FileName);
                var fileSize = file.Length;
                if (fileSize <= validFileSize)
                {

                    if (validFileExtension.Contains(extType))
                    {
                        string fileName = string.Format("{0}{1}", givenFileName + "_" + DateTime.Now.Millisecond, extType);
                        //create file path if it doesnt exist
                        if (!Directory.Exists(filePath))
                        {
                            Directory.CreateDirectory(filePath);
                        }
                        var fullPath = Path.Combine(filePath, fileName);
                        noteUrl = Path.Combine(directory, fileName);
                        //Delete if file exist
                        FileInfo fileExists = new FileInfo(fullPath);
                        if (fileExists.Exists)
                        {
                            fileExists.Delete();
                        }

                        using (var stream = new FileStream(fullPath, FileMode.Create))
                        {
                            await file.CopyToAsync(stream);
                        }

                        return noteUrl = noteUrl.Replace('\\', '/');


                    }
                    else
                    {
                        throw new BadImageFormatException("Invalid file type...Accepted formats are jpg, jpeg, and png");
                    }
                }
            }
            return noteUrl;
        }
        public async Task<IEnumerable<AssignmentListDto>> ListAssignmentByCourseId(long courseId)
        {
            if (courseId == 0)
                throw new NullReferenceException("No Course Id");
            return await _context.ASSIGNMENT.Where(f => f.CourseAllocation.CourseId == courseId && !f.IsDelete)
                .Include(f => f.CourseAllocation)
                .ThenInclude(c => c.Course)
                .ThenInclude(f => f.User)
                .ThenInclude(f => f.Person)
                .Select(f => new AssignmentListDto
                {
                    AssignmentId = f.Id,
                    Active = f.Active,
                    AssignmentName = f.AssignmentName,
                    CourseCode = f.CourseAllocation.Course.CourseCode,
                    CourseTitle = f.CourseAllocation.Course.CourseTitle,
                    DueDate = f.DueDate,
                    InstructorName = (f.CourseAllocation.Instructor.Person.Surname + " " + f.CourseAllocation.Instructor.Person.Firstname + " " + f.CourseAllocation.Instructor.Person.Othername),
                    IsPublished = f.PublishResult,
                    MaxScore = f.MaxScore
                })
                .ToListAsync();
        }

  
    }
}