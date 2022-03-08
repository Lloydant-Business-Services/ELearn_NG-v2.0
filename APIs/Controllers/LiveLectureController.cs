using BusinessLayer.Interface;
using DataLayer.Dtos;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace APIs.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LiveLectureController : ControllerBase
    {
        private readonly ILiveLectureService _service;
        private readonly IConfiguration _configuration;
        private readonly string key;
        public LiveLectureController(ILiveLectureService service, IConfiguration configuration)
        {
            _service = service;
            _configuration = configuration;
            key = _configuration.GetValue<string>("AppSettings:Key");

        }
        [HttpGet("[action]")]
        public async Task<string> ZoomSignIn() => await _service.ZoomSignIn();

        [HttpPost("[action]")]
        public async Task<int> OAuthRedirect(string code) => await _service.OAuthRedirect(code);

        [HttpPost("[action]")]
        public async Task<int> CreateMeeting(MeetingDto meetingDto) => await _service.CreateMeeting(meetingDto);

        [HttpGet("[action]")]
        public async Task<IEnumerable<MeetingDto>> GetMeetingByCourseId(long courseId) => await _service.GetMeetingByCourseId(courseId);

        [HttpGet("[action]")]
        public async Task<IEnumerable<MeetingDto>> GetLiveLecturesByInstructor(long InstructorId) => await _service.GetLiveLecturesByInstructor(InstructorId);
        [HttpGet("[action]")]
        public async Task<IEnumerable<MeetingDto>> GetAllLiveLectures() => await _service.GetAllLiveLectures();
        [HttpPost("[action]")]
        public async Task<int> DeleteLiveLecture(long LiveLectureId) => await _service.DeleteLiveLecture(LiveLectureId);


    }
}
