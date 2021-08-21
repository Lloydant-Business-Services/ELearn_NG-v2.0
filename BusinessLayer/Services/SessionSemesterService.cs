using BusinessLayer.Interface;
using DataLayer.Dtos;
using DataLayer.Model;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.Services
{
    public class SessionSemesterService : ISessionSemesterService
    {
        private readonly IConfiguration _configuration;
        private readonly ELearnContext _context;
        ResponseModel response = new ResponseModel();

        public SessionSemesterService(IConfiguration configuration, ELearnContext context)
        {
            _configuration = configuration;
            _context = context;
        }


        public async Task<ResponseModel> SetSessionSemester(long sessionId, long semesterId)
        {
            try
            {
                SessionSemester sessionSemester = new SessionSemester();
                Session session = new Session();
                Semester semester = new Semester();
                var doesExist = await _context.SESSION_SEMESTER.Where(f => f.SessionId == sessionId && f.SemesterId == semesterId).FirstOrDefaultAsync();
                if(doesExist != null)
                {
                    doesExist.SessionId = sessionId;
                    doesExist.SemesterId = semesterId;
                    doesExist.Active = true;
                    _context.Update(doesExist);
                    await _context.SaveChangesAsync();
                    await DeactivateOtherActiveSessionSemester(doesExist.Id);
                    //response.StatusCode = StatusCodes.Status208AlreadyReported;
                    //response.Message = "session/semester with sessionid and semesterid already set";
                    return response;
                }
                if(sessionId > 0 && semesterId > 0)
                {
                    session = await _context.SESSION.Where(s => s.Id == sessionId).FirstOrDefaultAsync();
                    semester = await _context.SEMESTER.Where(s => s.Id == semesterId).FirstOrDefaultAsync();
                    if(session != null && semester != null)
                    {
                        sessionSemester.SemesterId = semester.Id;
                        sessionSemester.SessionId = session.Id;
                        sessionSemester.Active = true;
                        _context.Add(sessionSemester);
                        await _context.SaveChangesAsync();
                        await DeactivateOtherActiveSessionSemester(sessionSemester.Id);
                        return response;
                    }
                }
                return null;

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        public async Task DeactivateOtherActiveSessionSemester(long activeSessionSemesterId)
        {
            var activeSessionSemester = await _context.SESSION_SEMESTER.Where(f => f.Active && f.Id != activeSessionSemesterId).ToListAsync();
            if (activeSessionSemester?.Count > 0)
            {
                foreach (var item in activeSessionSemester)
                {
                    var SessionSemester = await _context.SESSION_SEMESTER.Where(f => f.Id == item.Id).FirstOrDefaultAsync();
                    SessionSemester.Active = false;
                    _context.Update(SessionSemester);
                }
                await _context.SaveChangesAsync();
            }
        }

        public async Task<GetSessionSemesterDto> GetActiveSessionSemester()
        {
            return await _context.SESSION_SEMESTER.Where(a => a.Active)
                .Include(s => s.Semester)
                .Include(s => s.Session)
                .Select(f => new GetSessionSemesterDto { 
                    SemesterName = f.Semester.Name,
                    SessionName = f.Session.Name,
                    SemesterId = f.SemesterId,
                    SessionId = f.SessionId,
                    Id = f.Id
                })
                .FirstOrDefaultAsync();
        }
    }
}
