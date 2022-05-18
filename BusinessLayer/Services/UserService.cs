using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;
using BusinessLayer.Infrastructure;
using BusinessLayer.Interface;
using BusinessLayer.Services.Email.Interface;
using DataLayer.Dtos;
using DataLayer.Model;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using RestSharp;
using RestSharp.Authenticators;

namespace BusinessLayer.Services
{
    public class UserService : Repository<User>, IUserService
    {
        //private readonly ELearnContext _context;
        private readonly IConfiguration _configuration;
        private readonly IEmailService _emailService;
        private readonly string baseUrl;
        private readonly string defualtPassword = "1234567";
        ResponseModel response = new ResponseModel();

        public UserService(ELearnContext context, IConfiguration configuration, IEmailService emailService)
             : base(context)
        {
           // _context = context;
            _configuration = configuration;
            baseUrl = _configuration.GetValue<string>("Url:root");
            _emailService = emailService;



        }

        public async Task<UserDto> AuthenticateUser(UserDto dto, string injectkey)
        {
            var user = await _context.USER
               .Include(r => r.Role)
               .Include(r => r.Person)
               .Where(f => f.Active && f.Username == dto.UserName).Include(p => p.Person).FirstOrDefaultAsync();

            if (user == null)
                return null;
            if (!user.IsVerified)
                throw new Exception("Account has not been verified!");
            if (!VerifyPasswordHash(dto.Password, user.PasswordHash, user.PasswordSalt))
                throw new Exception("Inavlid Username/Password!"); ;
          
            var token = GenerateJSONWebToken(user);
            user.LastLogin = DateTime.UtcNow;
            _context.Update(user);
            await _context.SaveChangesAsync();
            //var token = tokenHandler.CreateToken(tokenDescriptor);
            //dto.Token = tokenHandler.WriteToken(token);
            dto.Token = token;
            dto.Password = null;
            dto.UserName = user.Username;
            dto.RoleName = user.Role.Name;
            dto.UserId = user.Id;
            dto.PersonId = user.PersonId;
            dto.FullName = user.Person.Surname + " " + user.Person.Firstname + " " + user.Person.Othername;
            dto.IsHOD = user.RoleId == 4 ? true : false;
            
            return dto;
        }


        private bool VerifyPasswordHash(string password, byte[] storedHash, byte[] storedSalt)
        {
            if (password == null) throw new ArgumentNullException("password");
            if (string.IsNullOrWhiteSpace(password)) throw new ArgumentException("Value cannot be empty or whitespace only string.", "password");
            if (storedHash.Length != 64) throw new ArgumentException("Invalid length of password hash (64 bytes expected).", "passwordHash");
            if (storedSalt.Length != 128) throw new ArgumentException("Invalid length of password salt (128 bytes expected).", "passwordHash");

            using (var hmac = new System.Security.Cryptography.HMACSHA512(storedSalt))
            {
                var computedHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
                for (int i = 0; i < computedHash.Length; i++)
                {
                    if (computedHash[i] != storedHash[i]) return false;
                }
            }

            return true;
        }

        //public async Task<long> PostUser(AddUserDto userDto)
        //{
        //    using var transaction = await _context.Database.BeginTransactionAsync();
        //    try
        //    {
        //        User user = new User();
        //        Person person = new Person()
        //            {
        //                Surname = userDto.Surname,
        //                Firstname = userDto.Firstname,
        //                Othername = userDto.Othername,
        //                Email = userDto.Email,
        //            };
        //            _context.Add(person);
        //        await _context.SaveChangesAsync();

        //        Utility.CreatePasswordHash(defualtPassword, out byte[] passwordHash, out byte[] passwordSalt);
        //            user.Username = userDto.Email;
        //            user.RoleId = userDto.RoleId;
        //            user.IsVerified = true;
        //            user.Active = true;
        //            user.PasswordHash = passwordHash;
        //            user.PasswordSalt = passwordSalt;
        //            user.PersonId = person.Id;
        //            _context.Add(user);
        //            await _context.SaveChangesAsync();

        //            await transaction.CommitAsync();

        //            return StatusCodes.Status200OK;
                    
        //    }
        //    catch (Exception ex)
        //    {
        //        transaction.Rollback();
        //        throw ex;
        //    }
           
        //}

        public async Task<bool> ChangePassword(ChangePasswordDto changePasswordDto)
        {
            if (changePasswordDto == null || changePasswordDto?.UserId == 0)
                throw new ArgumentNullException("Please, Provide UserID");
            var user = await _context.USER.Where(f => f.Id == changePasswordDto.UserId).FirstOrDefaultAsync();
            if (user == null)
                return false;
            if (!VerifyPasswordHash(changePasswordDto.OldPassword, user.PasswordHash, user.PasswordSalt))
                return false;
            Utility.CreatePasswordHash(changePasswordDto.NewPassword, out byte[] passwordHash, out byte[] passwordSalt);
            user.PasswordHash = passwordHash;
            user.PasswordSalt = passwordSalt;
            _context.Update(user);
            await _context.SaveChangesAsync();
            return true;

        }
        public async Task<GetUserProfileDto> GetUserProfile(long userId)
        {
            StudentPerson studentPerson = new StudentPerson();
            InstructorDepartment instructorDepartment = new InstructorDepartment();
            DepartmentHeads departmentHeads = new DepartmentHeads();
            User user = new User();
            GetUserProfileDto dto = new GetUserProfileDto();

            user = await _context.USER.Where(x => x.Id == userId).Include(p => p.Person).Include(r => r.Role).FirstOrDefaultAsync();
            if(user != null)
            {
                studentPerson = await _context.STUDENT_PERSON.Where(d => d.PersonId == user.PersonId).Include(x => x.Department).ThenInclude(f => f.FacultySchool).FirstOrDefaultAsync();
                instructorDepartment = await _context.INSTRUCTOR_DEPARTMENT.Where(i => i.UserId == user.Id).Include(d => d.Department).ThenInclude(f => f.FacultySchool).FirstOrDefaultAsync();
                departmentHeads = await _context.DEPARTMENT_HEADS.Where(d => d.UserId == user.Id).Include(d => d.Department).ThenInclude(f => f.FacultySchool).FirstOrDefaultAsync();

                dto.MatricNumber = studentPerson != null ? studentPerson.MatricNo : null;
                dto.Person = user.Person;
                if (studentPerson != null && studentPerson.DepartmentId > 0)
                {
                    dto.Department = studentPerson.Department;

                }
                else if (departmentHeads != null)
                {
                    dto.Department = departmentHeads.Department;
                }
                else if(instructorDepartment != null)
                {
                    dto.Department = instructorDepartment.Department;
                }
                
                dto.IsUpdatedProfile = user.IsVerified;
                dto.UserId = user.Id;
                dto.RoleName = user.Role.Name;
                dto.Username = user.Username;
            }
            return dto;

        }

        public async Task<ResponseModel> ProfileUpdate(UpdateUserProfileDto dto)
        {
            try
            {
                //StudentPerson studentPerson = new StudentPerson();
                User user = await _context.USER.Where(u => u.Id == dto.UserId).FirstOrDefaultAsync();
                if (user == null)
                    throw new NullReferenceException("User not found");
                Person person = await _context.PERSON.Where(p => p.Id == user.PersonId).FirstOrDefaultAsync();
                StudentPerson studentPerson = await _context.STUDENT_PERSON.Where(x => x.PersonId == person.Id).FirstOrDefaultAsync();

                if (dto.DepartmentId > 0 && studentPerson != null)
                {
                    studentPerson.DepartmentId = dto.DepartmentId;
                    _context.Update(studentPerson);

                }
                if (!String.IsNullOrEmpty(dto.Firstname))
                {
                    person.Firstname = dto.Firstname;

                }

                if (!String.IsNullOrEmpty(dto.Surname))
                {
                    person.Surname = dto.Surname;

                }
                if (!String.IsNullOrEmpty(dto.Othername))
                {
                    person.Othername = dto.Othername;

                }
                if (dto.GenderId > 0)
                {
                    person.GenderId = dto.GenderId;

                }
                if (!String.IsNullOrEmpty(dto.PhoneNumber))
                {
                    person.PhoneNo = dto.PhoneNumber;

                }
                if (!String.IsNullOrEmpty(dto.Email))
                {
                    person.Email = dto.Email;

                }


                _context.Update(person);
                //if(studentPerson != null)
                //{
                //    _context.Update(studentPerson);
                //}
                await _context.SaveChangesAsync();
                response.StatusCode = StatusCodes.Status200OK;
                response.Message = "success";
                return response;


            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<int> ResetPassword(string Username)
        {
            try
            {
                EmailDto emailDto = new EmailDto()
                {
                    ReceiverEmail = "miracleoghenemado@gmail.com"
                };
                var _username = Username.Trim();
                var getUser = await _context.USER.Where(x => x.Username == _username.Trim()).FirstOrDefaultAsync();
                if(getUser != null)
                {                   
                    string generateGuid = Convert.ToString(Guid.NewGuid());
                    var splitGuid = generateGuid.Split("-");
                    var guid = splitGuid[1];
                    getUser.Guid = guid;
                    await _emailService.EmailFormatter(emailDto);
                    //_context.Update(getUser);
                    //await _context.SaveChangesAsync();
                    return StatusCodes.Status200OK;
                }
                return 0;
            }
            catch(Exception ex)
            {
                throw ex;
            }
        }

        public async Task<bool> AscertainMultiRole(long userId, long sessionSemesterId)
        {
            var isDoubleRole = await _context.COURSE_ALLOCATION.Where(x => x.InstructorId == userId && x.SessionSemesterId == sessionSemesterId).ToListAsync();
            if (isDoubleRole != null && isDoubleRole.Count > 0) return true;
            else return false;
        }

        public static IRestResponse SendSimpleMessage(string to, string body)
        {
            string _body = "Your Verification code is : " + body;
            RestClient client = new RestClient();
            client.BaseUrl = new Uri("https://api.mailgun.net/v3/");
            client.Authenticator =
                new HttpBasicAuthenticator("api", "key-8540f3ef6a66cdaf8d9121f11c99aa6b");
            RestRequest request = new RestRequest();
            request.AddParameter("domain", "nrf.lloydant.com", ParameterType.UrlSegment);
            request.Resource = "{domain}/messages";
            request.AddParameter("from", "ABSU Elearn NG <mailgun@absuelearn.com>");
            request.AddParameter("to", to);
            //request.AddParameter("to", "YOU@YOUR_DOMAIN_NAME");
            request.AddParameter("subject", "Account Verification");
            request.AddParameter("text", _body);
            request.Method = Method.POST;
            var stat = client.Execute(request);
            return stat;
        }
        private string GenerateJSONWebToken(User userInfo)
        {
            var expiryDate = DateTime.Now.AddHours(48);
            ClaimsIdentity claims = new ClaimsIdentity(new Claim[]
                {
                    new Claim(CustomClaim.USER_ID, userInfo.Id.ToString()),
                    new Claim(CustomClaim.USER_ROLE, userInfo.Role.Id.ToString()),
                    new Claim(CustomClaim.USER_NAME, userInfo.Username),
                    new Claim(CustomClaim.NAME, $"{userInfo.Person.Firstname}  {userInfo.Person.Othername}"),
                    new Claim(CustomClaim.TOKEN_EXPIRY_DATE, expiryDate.ToString("yyyy-MM-dd")),
                    new Claim(CustomClaim.TOKEN_ISSUANCE_DATE, DateTime.Now.ToString("yyyy-MM-dd"))
                });

            JwtSecurityTokenHandler tokenHandler = new JwtSecurityTokenHandler();
            byte[] key = Encoding.ASCII.GetBytes(_configuration["AppSettings:Key"]);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = claims,
                Expires = expiryDate,
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }

    }

}
